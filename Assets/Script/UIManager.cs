using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour {

    static List<Text> texts;
    static List<NumberDisplay> numberDisplays;

	// Use this for initialization
	void Start () {
        Debug.Log("uimanager");
        texts = new List<Text>();
        numberDisplays = new List<NumberDisplay>();

        GameObject[] uis = GameObject.FindGameObjectsWithTag("ui");
        foreach(GameObject ui in uis)
        {
            if (ui.GetComponent<Text>() != null)
            {
                texts.Add(ui.GetComponent<Text>());
            }
            if(ui.GetComponent<NumberDisplay>() != null)
            {
                numberDisplays.Add(ui.GetComponent<NumberDisplay>());
            }
        }

        this.ObserveEveryValueChanged(score => PresentGameManager.score)
        .Subscribe(score => {
            UIManager.SetText("BallCountText", "Score：" + PresentGameManager.score);
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SetText(string name, string value)
    {
        texts.Find(t => t.gameObject.name == name).text = value;
    }
}
