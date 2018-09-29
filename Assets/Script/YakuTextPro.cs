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
        presentManager.OnMakeYaku.Subscribe(madeYaku =>
        {
            switch (madeYaku.basketType) {
                case BasketType.Normal:
                text.text = madeYaku.yaku.yakuName + "　+" + madeYaku.yaku.score;
                Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
                {
                    text.text = "";
                }).AddTo(this);
                    break;
                case BasketType.X2:
                    text.text = madeYaku.yaku.yakuName + "　+" + madeYaku.yaku.score + "× 2";
                    Observable.Timer(TimeSpan.FromSeconds(1.5f)).Subscribe(_ =>
                    {
                        text.text = "";
                    }).AddTo(this);
                    break;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
