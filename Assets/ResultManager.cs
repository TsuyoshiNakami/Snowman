using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour {
    [SerializeField] GameObject resultWindow;

    [SerializeField] GameObject resultElement;
    [SerializeField] Transform resultTransform;
    [SerializeField] TextMeshProUGUI sumText;
    PresentManager presentManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowResult()
    {

        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        foreach (YakuResult result in presentManager.yakuResults) {
            GameObject newResult = Instantiate(resultElement, resultTransform);
            newResult.GetComponent<ResultElement>().SetUI(result);
        }
        sumText.text = "合計：" + PresentGameManager.score;
    }
}
