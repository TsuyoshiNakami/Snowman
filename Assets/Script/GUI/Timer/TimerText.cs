using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TimerState {
	NONE,
	ACTIVE,
	OVER,
}

public class TimerText : MonoBehaviour {
	Text text;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		Timer.UpdateTimer ();
		if (Timer.state == TimerState.ACTIVE || Timer.state == TimerState.OVER) {
			text.enabled = true;
			text.text = Mathf.Ceil (Timer.time).ToString ();
		} else {
			text.enabled = false;

		}
	}
}

public  class Timer {
	public static TimerState state = TimerState.NONE;
	public static float time = 0;
	static float settedTime = 0;
	public static void SetTime(float f) {
		settedTime = f;
	}

	public static void StartTimer() {
		time = settedTime;
		state = TimerState.ACTIVE;
	}

	public static void UpdateTimer() {
		if (state != TimerState.ACTIVE) {
			return;
		}
		time -= Time.deltaTime;
		if (time <= 0) {
			EndTimer ();
		}	
	}

	static void EndTimer() {
		state = TimerState.OVER;
	}

	public static void InitTimer() {
		state = TimerState.NONE;
	}
}