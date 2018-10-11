using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TransitionButton : MonoBehaviour {

    [SerializeField] GameScenes scene;
    [SerializeField] bool focused = false;
    SoundManager soundManager;

	// Use this for initialization
	void Start () {
        soundManager = GameObject.Find("SoundManager"). GetComponent<SoundManager>();
        if (focused)
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        soundManager.PlaySEOneShot("Decide");
        GameManager.LoadScene(scene);
    }
}
