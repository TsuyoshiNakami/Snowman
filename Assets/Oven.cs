using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour {
    Present present = null;
    float bakeTime = 0;
    public void PutIn(Present present)
    {
        if (this.present == null)
        {
            present.GetComponent<Throwable>().OnHeld(gameObject);
            this.present = present;
        }
    }

    public void PutOut()
    {
        if(present == null)
        {
            return;
        }
        if (present.GetComponent<Bakeable>() != null)
        {
            present.GetComponent<Bakeable>().Bake(bakeTime);
        }
        present.GetComponent<Throwable>().OnRelease();
        present = null;
        Debug.Log("焼いた時間：" + bakeTime);
        bakeTime = 0;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(present != null)
        {
            bakeTime += Time.deltaTime;
        }
	}
}
