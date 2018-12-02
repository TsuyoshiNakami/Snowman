using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TitleState
{
    PressStart,
    Menu,
    StageSelect
}
public class TitleManager : MonoBehaviour {

    TitleState state = TitleState.PressStart;
    [SerializeField] GameObject pressStartText;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject titleUI;

    [SerializeField]GameObject startButton;
    SoundManager soundManager; 
	// Use this for initialization
	void Start () {
        soundManager = GameObject.Find("SoundManager"). GetComponent<SoundManager>();

        if (!ES3.KeyExists("Tutorial"))
        {
            Destroy(GameObject.Find("Main Camera"));
            SceneManager.LoadScene("Opening", LoadSceneMode.Additive);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Home") && state == TitleState.PressStart)
        {
            state = TitleState.Menu;
            pressStartText.SetActive(false);
            buttons.SetActive(true);
            soundManager.PlaySEOneShot("Decide");
                 EventSystem.current.SetSelectedGameObject(startButton);
                startButton.GetComponent<Button>().OnSelect(null);           
        }

        if(Input.GetButtonDown(KeyConfig.Cancel))
        {
            if (state == TitleState.Menu)
            {
                state = TitleState.PressStart;
                pressStartText.SetActive(true);
                buttons.SetActive(false);

            }
            
            if(state == TitleState.StageSelect)
            {
                state = TitleState.Menu;
                buttons.SetActive(true);
                EventSystem.current.SetSelectedGameObject(startButton);
                startButton.GetComponent<Button>().OnSelect(null);
                SceneManager.UnloadSceneAsync("StageSelect");
            }
        }

	}

    public void OnGameStartButtonClicked()
    {
        if (ES3.KeyExists("Tutorial"))
        {
            buttons.SetActive(false);
            state = TitleState.StageSelect;
            SceneManager.LoadScene("StageSelect", LoadSceneMode.Additive);
        }
        else
        {
            titleUI.SetActive(false);
            GameObject.Find("OpeningManager").GetComponent<OpeningManager>().StartOpening();
        }
    }
}
