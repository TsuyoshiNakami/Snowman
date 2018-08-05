using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class YakuText : MonoBehaviour {

    [SerializeField]BasketPresentViewer viewer;
    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        text.text = "";

        viewer.OnMakeYaku.Subscribe(yakuName =>
        {
            text.text = yakuName;
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
