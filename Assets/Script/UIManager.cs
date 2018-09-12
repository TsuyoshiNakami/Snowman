using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class UIManager : MonoBehaviour {

    static List<Text> texts;

	// Use this for initialization
	void Start () {
        texts = new List<Text>();
        GameObject[] uis = GameObject.FindGameObjectsWithTag("ui");
        foreach(GameObject ui in uis)
        {
            texts.Add(ui.GetComponent<Text>());
        }

        this.ObserveEveryValueChanged(score => GameManager.score)
        .Subscribe(score => {
            UIManager.SetText("BallCountText", "Score：" + GameManager.score);
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
