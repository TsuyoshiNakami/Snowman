using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : MonoBehaviour {
    [SerializeField] GameObject pauseWindowObj;
    PresentGameManager presentGameManager;
    GameManager gameManager;
	// Use this for initialization
	void Start () {
        presentGameManager = GameObject.Find("PresentGameManager").GetComponent<PresentGameManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseWindowObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown(KeyConfig.Home))
        {
            
            if (pauseWindowObj.activeInHierarchy)
            {
                Pauser.Resume();
                pauseWindowObj.SetActive(false);
            } else
            {
                if (!Pauser.isPausing)
                {
                    Pauser.Pause(PauseMode.Force);
                    pauseWindowObj.SetActive(true);
                }
            }
        }


        if(Input.GetButtonDown(KeyConfig.Decide))
        {
            if (pauseWindowObj.activeInHierarchy)
            {

                GameManager.LoadScene(GameScenes.Title);
            }
        }
	}
}
