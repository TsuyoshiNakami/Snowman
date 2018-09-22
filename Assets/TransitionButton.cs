using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TransitionButton : MonoBehaviour {

    [SerializeField] GameScenes scene;
    [SerializeField] bool focused = false;

	// Use this for initialization
	void Start () {
        if(focused)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        GameManager.LoadScene(scene);
    }
}
