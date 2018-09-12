using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTossDirector : MonoBehaviour {

    float limitTime = 30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        limitTime -= Time.deltaTime;
        if (limitTime > 0)
        {
            UIManager.SetText("TimerText", "残り" + Mathf.Ceil(limitTime) + "秒");
        } else
        {
            UIManager.SetText("TimerText", "終わり");
        }
	}
}
