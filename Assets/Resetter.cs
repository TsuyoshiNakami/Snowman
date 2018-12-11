using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetter : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
    int count = 0;
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            count++;
            if(count > 10)
            {
                ES3.DeleteFile();
                SceneManager.LoadScene("Title");
                
            }
        }
	}
}
