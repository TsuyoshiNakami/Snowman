
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

public enum BasketType
{
    Normal,
    X2
}
public class BasketPresentViewer : MonoBehaviour
{
    [SerializeField]
    GameObject viewStart;
    [SerializeField]
    GameObject viewEnd;

    List<GameObject> presentObjs;
    BasketCollider basketCollider;
    List<Present> presents = new List<Present>();
    [Inject]
    PresentManager presentManager;
    GameManager gameManager;
    [SerializeField] BasketType basketType;
    [SerializeField] GameObject presentMemoObj;

    [SerializeField] float viewPresentSizeDivider = 3;
    Subject<string> makeYakuSubject = new Subject<string>();

    bool blockPresent = false;

    public IObservable<string> OnMakeYaku
    {
        get
        {
            return makeYakuSubject;
        }
    }
    Subject<GameObject> presentEnterSubject = new Subject<GameObject>();
    public IObservable<GameObject> OnPresentEnter
    {
        get
        {
            return presentEnterSubject;
        }
    }


    Subject<Unit> madePresentSubject = new Subject<Unit>();
    public IObservable<Unit> OnMadePresent
    {
        get
        {
            return madePresentSubject;
        }
    }

    // Use this for initialization
    void Start()
    {
        presentObjs = new List<GameObject>();
        basketCollider = GetComponent<BasketCollider>();
        basketCollider.OnItemEnter.Subscribe(item =>
        {
            if (!blockPresent)
            {
                Destroy(item);
                ViewPresent(item);
            }
        });

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }


    public void ViewPresent(GameObject item)
    {
        GetComponent<Animator>().SetTrigger("EnterPresent");
        presentManager.HidePresentFromView(item);
        presentEnterSubject.OnNext(item);
        Present present = item.GetComponent<Present>();

        GameObject newObj = Instantiate(presentMemoObj, transform);

        Present newPresent = newObj.AddComponent<Present>();
        newPresent.presentName = present.presentName;
        newPresent.attributes = present.attributes;
        newPresent.completeYaku = present.completeYaku;
        presents.Add(newPresent);
        newObj.GetComponent<PresentMemo>().SetPresentContent(item.GetComponent<SpriteRenderer>().sprite);
        presentObjs.Add(newObj);

        Vector2 viewLine = viewEnd.transform.position - viewStart.transform.position;
        for (int i = 0; i < presentObjs.Count; i++)
        {
            presentObjs[i].transform.position = (Vector2)viewStart.transform.position + viewLine / (presentObjs.Count + 1) * (i + 1);
            //presentObjs[i].transform.localScale = Vector3.one / viewPresentSizeDivider;
        }
        if (presentObjs.Count == 1)
        {
            gameManager.PlaySE("PresentEnter1");
        }
        else if (presentObjs.Count == 2)
        {

            gameManager.PlaySE("PresentEnter2");
        }

        if (presentObjs.Count >= 3)
        {
            OnEnterPresent();
        }
    }

    void ClearPresents()
    {
        foreach (GameObject obj in presentObjs)
        {
            Destroy(obj);
        }
        presentObjs.Clear();
        presents.Clear();

        Destroy(gameObject);
    }
    void OnEnterPresent()
    {
        blockPresent = true;
        GetComponent<Animator>().SetBool("IsFinished", true);
        YakuList yakuList = GameObject.Find("YakuList").GetComponent<YakuList>();
        Yaku maxYaku = PresentUtility.DistinguishYaku(presents, yakuList);

        if (maxYaku != null)
        {
            ES3.Save<bool>(maxYaku.yakuName, true, PresentGameConsts.saveSetting);
        }
        else
        {
            maxYaku = yakuList.defaultYaku;
        }

        //役完成時のイベント
        presentManager.OnMakeYakuEvent(presents, maxYaku, basketType);
        makeYakuSubject.OnNext(maxYaku.yakuName);
        PlaySeByScore(maxYaku.score);
    }

    public void OnMadePresentBox()
    {
        madePresentSubject.OnNext(Unit.Default);
        ClearPresents();
    }

    void PlaySeByScore(int score)
    {
        if (score < 100)
        {
            gameManager.PlaySE("PresentEnter3");
        }
        else
        {
            gameManager.PlaySE("GoodPresent1");
        }
    }
}
