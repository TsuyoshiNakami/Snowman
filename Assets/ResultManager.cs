using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class ResultManager : MonoBehaviour {
    [SerializeField] GameObject resultWindow;

    [SerializeField] GameObject resultElement;
    [SerializeField] Transform resultTransform;
    [SerializeField] TextMeshProUGUI sumText;
    [SerializeField] Button[] buttons;
    [Inject]
    PresentManager presentManager;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowResult()
    {

        foreach (YakuResult result in presentManager.yakuResults) {
            GameObject newResult = Instantiate(resultElement, resultTransform);
            newResult.GetComponent<ResultElement>().SetUI(result);
        }
        sumText.text = "合計：" + PresentGameManager.score;
    }

    public void SetButtonsInteractive(bool f)
    {
        for(int i = 0; i < buttons.Length; i++) 
        {
            buttons[i].interactable = f;
        }
    }

    public void InitButtonFocus()
    {
        EventSystem.current.SetSelectedGameObject(buttons[1].gameObject);
    }
} 
