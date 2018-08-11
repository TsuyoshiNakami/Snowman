using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class YakuText : MonoBehaviour {

    PresentManager presentManager;

    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = "";
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        presentManager.OnMakeYaku.Subscribe(yaku =>
        {

            Debug.Log(yaku.yakuName + " +" + yaku.score);
            text.text = yaku.yakuName + "　+" + yaku.score;
            Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
            {
                text.text = "";
            }).AddTo(this);
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
