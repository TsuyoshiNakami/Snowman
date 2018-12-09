using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tsuyomi.Yukihuru.Scripts.Utilities;

public class TransitionButton : MonoBehaviour {

    [SerializeField] GameScenes scene;
    [SerializeField] bool focused = false;
    SoundManager soundManager;

	// Use this for initialization
	void Start () {
        soundManager = GameObject.Find("SoundManager"). GetComponent<SoundManager>();
        if (focused)
        {
            Debug.Log(this.name + " : focus");
            //EventSystem.current.SetSelectedGameObject(this.gameObject);
            this.GetComponent<Button>().Select();
            this.GetComponent<Button>().OnSelect(null);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        soundManager.PlaySEOneShot("Decide");
        switch (scene)
        {
            case GameScenes.Title:
                soundManager.FadeOut(1);
                    SceneLoader.LoadScene(scene);
                break;
            case GameScenes.Tutorial:
                if (ES3.Load<bool>("Tutorial", false))
                {
                    SceneLoader.LoadScene(GameScenes.GameEasy);
                }
                else
                {
                   SceneLoader.LoadScene(GameScenes.Tutorial);
                }
                break;
            default:
                    SceneLoader.LoadScene(scene);
                break;
        }
    }
}
