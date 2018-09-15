using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

public class YakuTextPro : MonoBehaviour
{

    PresentManager presentManager;

    [SerializeField]TextMeshProUGUI text;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "";
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        presentManager.OnMakeYaku.Subscribe(yaku =>
        {
            
            text.text = yaku.yakuName + "　+" + yaku.score;
            Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
            {
                text.text = "";
            }).AddTo(this);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
