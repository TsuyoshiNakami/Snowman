using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TitleState
{
    PressStart,
    Menu
}
public class TitleManager : MonoBehaviour {

    TitleState state = TitleState.PressStart;
    [SerializeField] GameObject pressStartText;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject titleUI;
    SoundManager soundManager; 
	// Use this for initialization
	void Start () {
        soundManager = GameObject.Find("SoundManager"). GetComponent<SoundManager>();

        SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Home") && state == TitleState.PressStart)
        {
            state = TitleState.Menu;
            pressStartText.SetActive(false);
            buttons.SetActive(true);
            soundManager.PlaySEOneShot("Decide");
            
        }

        if(Input.GetButtonDown(KeyConfig.Cancel) && state == TitleState.Menu)
        {
            state = TitleState.PressStart;
            pressStartText.SetActive(true);
            buttons.SetActive(false);
        }

	}

    public void OnGameStartButtonClicked()
    {
        titleUI.SetActive(false);
        GameObject.Find("TutorialManager").GetComponent<TutorialManager>().StartTutorial();
    }
}
