﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BasketPresentViewer : MonoBehaviour {
    [SerializeField]
    GameObject viewStart;
    [SerializeField]
    GameObject viewEnd;

    List<GameObject> presentObjs;
    BasketCollider basketCollider;
    List<Present> presents = new List<Present>();
    PresentManager presentManager;
    GameManager gameManager;
    

    Subject<string> makeYakuSubject = new Subject<string>();
    public IObservable<string> OnMakeYaku
    {
        get
        {
            return makeYakuSubject;
        }
    }
    // Use this for initialization
    void Start () {
        presentObjs = new List<GameObject>();
        basketCollider = GetComponent<BasketCollider>();
        basketCollider.OnItemEnter.Subscribe(item => {
            ViewPresent(item);
        });

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
    }
	

    public void ViewPresent(GameObject item)
    {
        presentManager.HidePresentFromView(item);
        Present present = item.GetComponent<Present>();

        GameObject newObj = new GameObject();
        newObj.transform.parent = transform;
        newObj.transform.localScale = Vector3.one / 2;

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();

        newObj.name = "EnterItem";
        Present newPresent = newObj.AddComponent<Present>();
        newPresent.presentName = present.presentName;
        newPresent.attributes = present.attributes;
        newPresent.completeYaku = present.completeYaku;
        presents.Add(newPresent);
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 100;
        presentObjs.Add(newObj);

        Vector2 viewLine = viewEnd.transform.position - viewStart.transform.position;
        for(int i = 0; i < presentObjs.Count; i++)
        {
            presentObjs[i].transform.position = (Vector2)viewStart.transform.position + viewLine / (presentObjs.Count + 1) * (i + 1);
        }
        if(presentObjs.Count == 1)
        {
            gameManager.PlaySE("PresentEnter1");
        } else if (presentObjs.Count == 2)
        {

            gameManager.PlaySE("PresentEnter2");
        }

        if(presentObjs.Count >= 3)
        {
            OnEnterPresent();
        }
    }

    void ClearPresents()
    {
        foreach(GameObject obj in presentObjs)
        {
            Destroy(obj);
        }
        presentObjs.Clear();
        presents.Clear();
    }
    void OnEnterPresent()
    {
        YakuList yakuList = GameObject.Find("YakuList").GetComponent<YakuList>();
        Yaku maxYaku = PresentUtility.DistinguishYaku(presents, yakuList);


        if (maxYaku != null)
        {
            PresentGameManager.score += maxYaku.score;
            presentManager.OnMakeYakuEvent(maxYaku);
            makeYakuSubject.OnNext(maxYaku.yakuName);
            PlaySeByScore(maxYaku.score);

            ES3.Save<bool>(maxYaku.yakuName, true, PresentGameConsts.saveSetting);
        }
        else
        {
            PresentGameManager.score += yakuList.defaultYaku.score;
            presentManager.OnMakeYakuEvent(yakuList.defaultYaku);
            PlaySeByScore(yakuList.defaultYaku.score);
        }


        Invoke("ClearPresents", 0f);

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
