using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTestText : MonoBehaviour {

    Text text;
	void Start()
    {
        text = GetComponent<Text>();
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            text.text = "ボタン";
        } else
        {
            text.text = "";
        }

    }
}
