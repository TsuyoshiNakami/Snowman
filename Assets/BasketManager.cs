using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BasketManager : MonoBehaviour {

    [SerializeField] int basketX2AppearanceDivider = 3;
    [SerializeField] GameObject basket;
    [SerializeField] GameObject basketX2;
    [SerializeField] GameObject generatePosition;

    int generateCount = 0;
    int x2appearTiming = 0;
	// Use this for initialization
	void Start () {
        // 何回目に2倍カゴが現れるか    初回は出さない
        x2appearTiming = Random.Range(1, basketX2AppearanceDivider);
        // X2カゴのタイミングならX2、それ以外なら普通のカゴを生成
        if (generateCount == x2appearTiming)
        {
            GenerateBasket(basketX2);
        }
        else
        {
            GenerateBasket(basket);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnPresentCompleted()
    {
        generateCount++;
        //Debug.Log(generateCount);
        // 設定回数に達したら
        if(generateCount >= basketX2AppearanceDivider)
        {

            generateCount = 0;
            x2appearTiming = Random.Range(0, basketX2AppearanceDivider);
            //Debug.Log("カウント初期化　次の2倍カゴは：" + x2appearTiming);
        }

        // X2カゴのタイミングならX2、それ以外なら普通のカゴを生成
        if (generateCount == x2appearTiming)
        {
            GenerateBasket(basketX2);
        }
        else
        {
            GenerateBasket(basket);
        }
    }

    void GenerateBasket(GameObject basket)
    {
        GameObject newObj = Instantiate(basket, generatePosition.transform.position, generatePosition.transform.rotation);
        newObj.GetComponentInChildren<BasketPresentViewer>().OnMakeYaku.Subscribe(yaku => {
            OnPresentCompleted();
        });
    }
}
