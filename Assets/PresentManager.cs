using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PresentManager : MonoBehaviour {
    [SerializeField] List<GameObject> presents;
    List<GameObject> presentsInView = new List<GameObject>();

    Subject<Yaku> makeYakuSubject = new Subject<Yaku>();
    public IObservable<Yaku> OnMakeYaku
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

        int index = Random.Range(0, presents.Count);
        GameObject newPresentObj = Instantiate(presents[index], pos, transform.rotation);

        presentsInView.Add(newPresentObj);
        return newPresentObj;
    }

    public void HidePresentFromView(GameObject hideObj)
    {
        presentsInView.Remove(hideObj);
    }

    public void OnMakeYakuEvent(Yaku yaku)
    {
        makeYakuSubject.OnNext(yaku);
    }
}
