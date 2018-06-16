using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEnterFirePlace : MonoBehaviour {
	public float time = 30;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if (Timer.state == TimerState.NONE && MessageWindowController.msgWindowInfo == "Msg_EnterFirePlace_End") {
			Timer.SetTime (time);
			Timer.StartTimer ();
			MessageWindowController.clearInfo ();
		}

		if (Timer.state == TimerState.OVER) {
			GameObject.Find ("Player").GetComponent<PlayerController> ().Damage (4);
		}
	}
}
