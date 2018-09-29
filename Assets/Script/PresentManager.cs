using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MadeYaku
{
    public Yaku yaku;
    public BasketType basketType;

    public MadeYaku(Yaku yaku, BasketType basketType)
    {
        this.yaku = yaku;
        this.basketType = basketType;
    }
}

public class PresentManager : MonoBehaviour {
    [SerializeField] List<GameObject> kindOfPresents;
    List<GameObject> presentsInView = new List<GameObject>();

    Subject<MadeYaku> makeYakuSubject = new Subject<MadeYaku>();
    public IObservable<MadeYaku> OnMakeYaku
    {
        get
        {
            return makeYakuSubject;
        }
    }

    public int NumberOfPresentInView
    {
        get
        {
            return presentsInView.Count;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject EmitPresentRandom(Vector2 pos)
    {

        int index = Random.Range(0, kindOfPresents.Count);
        GameObject newPresentObj = Instantiate(kindOfPresents[index], pos, transform.rotation);

        presentsInView.Add(newPresentObj);
        return newPresentObj;
    }

    public void HidePresentFromView(GameObject hideObj)
    {
        presentsInView.Remove(hideObj);
    }

    public void OnMakeYakuEvent(Yaku yaku, BasketType type)
    {
        makeYakuSubject.OnNext(new MadeYaku(yaku, type));
        switch(type)
        {
            case BasketType.Normal:
                PresentGameManager.score += yaku.score;
                break;
            case BasketType.X2:
                PresentGameManager.score += yaku.score * 2;
                break;
        }

    }

    public void DeleteAllPresents()
    {
        foreach(GameObject present in presentsInView)
        {
            Destroy(present);
        }
        presentsInView.Clear();
    }

    public void ChangePresent(GameObject from, GameObject to)
    {
        presentsInView.Remove(from);
        presentsInView.Add(to);
    }
}
