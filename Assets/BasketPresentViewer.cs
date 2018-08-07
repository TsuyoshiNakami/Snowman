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
	}
	

    public void ViewPresent(GameObject item)
    {
        Present present = item.GetComponent<Present>();

        Debug.Log(present.attributes);
        GameObject newObj = Instantiate(new GameObject(), transform);
        newObj.transform.localScale = Vector3.one / 2;

        SpriteRenderer spriteRenderer = newObj.AddComponent<SpriteRenderer>();

        newObj.name = "EnterItem";
        Present newPresent = newObj.AddComponent<Present>();
        newPresent.presentName = present.presentName;
        newPresent.attributes = present.attributes;

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

        if(presentObjs.Count >= 3)
        {
            DistinguishYaku();
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
    void DistinguishYaku()
    {
        YakuList yakuList = GameObject.Find("YakuList").GetComponent<YakuList>();

        foreach(Yaku yaku in yakuList.yakus)
        {

            List<Present> tmpPresents = new List<Present>(presents);
            
            foreach (Present yakuElement in yaku.presents)
            {
                Debug.Log(yakuElement + "は入ってるかな？");
                bool foundFlag = false;
                Present foundPresent = null;
                foreach(Present present in tmpPresents)
                {
                    Debug.Log("Yaku Element : " + yakuElement.AttributesToUInt() + ", present : " + present.AttributesToUInt());
                    // presentの中に aフラグが含まれているか？
                    if(present.MeetConditions(yakuElement.AttributesToUInt())) {
                        Debug.Log(yakuElement + "は入ってる");
                        foundFlag = true;
                        foundPresent = present;
                        Debug.Log("見つかったプレゼント：" + foundPresent);
                        break;
                    }
                    if (foundFlag)
                    {
                        break;
                    }
                }

                if(foundPresent != null)
                {

                    Debug.Log(foundPresent + "は除外する");
                    tmpPresents.Remove(foundPresent);
                }
                if (tmpPresents.Count == 0)
                {
                    GameManager.score += yaku.score;
                    makeYakuSubject.OnNext(yaku.yakuName);
                    break;
                }
            }
        }
        Invoke("ClearPresents", 1.5f);

    }
}
