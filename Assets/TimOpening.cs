using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TimOpening : MonoBehaviour {
    Subject<Unit> suprisedBySnowmanSubject = new Subject<Unit>();

    public IObservable<Unit> OnSuprisedBySnowman
    {
        get { return suprisedBySnowmanSubject; }
    }

    public void SuprisedBySnowman()
    {
        suprisedBySnowmanSubject.OnNext(Unit.Default);
    }
	
}
