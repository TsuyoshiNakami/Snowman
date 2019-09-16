using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Zenject;
using Random = UnityEngine.Random;

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

public struct YakuResult
{
    public Yaku yaku;
    public List<Present> presents;
    public int count;


    public YakuResult(Yaku yaku, List<Present> presents)
    {
        this.yaku = yaku;
        List<Present> newPresents = new List<Present>();
        foreach(Present p in presents)
        {
            Present tmp = new Present();
            tmp.presentName = p.presentName;
            tmp.sprite = p.GetComponent<SpriteRenderer>().sprite;
            newPresents.Add(tmp);
        }
        this.presents = newPresents;
        count = 1;
    }
}
public class PresentManager : MonoBehaviour {
    public bool autoDisappearPresent;
    public float presentDisappearTime = 6;
    public float flashTime = 3;
    [SerializeField] List<GameObject> kindOfPresents;
    public List<YakuResult> yakuResults = new List<YakuResult>();
    List<GameObject> presentsInView = new List<GameObject>();
        [Inject]
    DiContainer diContainer;
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


    public GameObject EmitPresentRandom(Vector2 pos)
    {

            
        int index = Random.Range(0, kindOfPresents.Count);
        GameObject newPresentObj = diContainer.InstantiatePrefab(kindOfPresents[index]);
        newPresentObj.transform.position = pos;
        newPresentObj.transform.localScale = Vector3.one;

        //Instantiate(, pos, transform.rotation);

        presentsInView.Add(newPresentObj);
        return newPresentObj;
    }

    public List<Present> GetPresentByAttribute(PresentAttribute attribute)
    {
        List<Present> tmp = new List<Present>();
        foreach (GameObject obj in presentsInView)
        {
            if (obj.GetComponent<Present>().MeetConditions((uint)attribute.GetHashCode())) {
                tmp.Add(obj.GetComponent<Present>());
            }
        }
        return tmp;
    }
    public void HidePresentFromView(GameObject hideObj)
    {
        presentsInView.Remove(hideObj);
    }

    public void OnMakeYakuEvent(List<Present> presents, Yaku yaku, BasketType type)
    {
        YakuResult yakuResult = new YakuResult(yaku, presents);

        YakuResult existYaku = yakuResults.Find(exist => exist.yaku.yakuName == yakuResult.yaku.yakuName);
        //　今回のプレイで初めて作った役
        if (existYaku.yaku == null)
        {
            yakuResults.Add(yakuResult);
        } else
        {
            yakuResults.Remove(existYaku);
            switch (type)
            {
                case BasketType.Normal:
                    existYaku.count++;
                    break;
                case BasketType.X2:
                    existYaku.count += 2;
                    break;
            }

            yakuResults.Add(existYaku);
        }

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

    public void DeletePresent(GameObject presentObj)
    {
        Destroy(presentObj);
        presentsInView.Remove(presentObj);
    }
    public void ChangePresent(GameObject from, GameObject to)
    {
        presentsInView.Remove(from);
        presentsInView.Add(to);
    }
}
