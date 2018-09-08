using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TimerState {
	NONE,
	ACTIVE,
	OVER,
}

public class TimerText : MonoBehaviour {
    PresentGameManager presentGameManager;
	TextMeshProUGUI text;
	// Use this for initialization
	void Start () {
		text = GetComponent<TextMeshProUGUI> ();
        presentGameManager = GameObject.Find("PresentGameManager").GetComponent<PresentGameManager>();
	}

	// Update is called once per frame
	void Update () {
        text.text = "" + Mathf.Ceil(presentGameManager.TimeLimit);

	}
}

