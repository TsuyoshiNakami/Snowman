using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MessageWindow : MonoBehaviour {

    //[SerializeField] TextMeshProUGUI text;
    [SerializeField] TMP_Text text;

	// Use this for initialization
	void Start () { 

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string _text)
    {
        text.text = _text;

    }
}
