using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BasketManager : MonoBehaviour {

    [SerializeField] int basketX2AppearanceDivider = 3;
    [SerializeField] GameObject basket;
    [SerializeField] GameObject basketX2;
    List<Transform> generatePositions = new List<Transform>();
    List<bool> basketExistence = new List<bool>();

    int generateCount = 0;
    int x2appearTiming = 0;
	// Use this for initialization
	void Start () {

        //バスケット出現位置
        foreach(Transform child in GameObject.Find("BasketGeneratePoints").transform)
        {
            generatePositions.Add(child);
            basketExistence.Add(false);
        }


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
           GenerateBasket(basket);        OnPresentCompleted();
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
        //int i = Random.Range(0, generatePositions.Count);
        int i = 0;
        while(basketExistence[i])
        {
            i++;
            if(basketExistence.Count <= i)
            {
                return;
            }
        }
        basketExistence[i] = true;
        GameObject newObj = Instantiate(basket, generatePositions[i].position, generatePositions[i].rotation);
        newObj.GetComponentInChildren<BasketPresentViewer>().OnMakeYaku.Subscribe(yaku => {
            basketExistence[i] = false;
            OnPresentCompleted();
        });
    }
}
