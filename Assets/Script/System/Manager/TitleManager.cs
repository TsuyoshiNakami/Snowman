using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tsuyomi.Yukihuru.Scripts.Utilities;
using naichilab;
using UniRx;
using Rewired;

public enum TitleState
{
    PressStart,
    Menu,
    StageSelect
}
public class TitleManager : MonoBehaviour
{

    TitleState state = TitleState.PressStart;
    [SerializeField] GameObject pressStartText;
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject titleUI;

    [SerializeField] GameObject startButton;
    [SerializeField] GameObject titleImage;
    [SerializeField] GameObject logoImage;
    [SerializeField] GameObject openingButton;

    bool isRankingOpen;
    SoundManager soundManager;
    Player player;

    // Use this for initialization
    void Start()
    {
        player = ReInput.players.GetPlayer(0);

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        GameObject.Find("RankingLoader").GetComponent<RankingLoader>().OnCloseRanking.Subscribe(_ => {
            OnCloseRanking();
        });
        if (!ES3.KeyExists("Tutorial"))
        {
            Destroy(GameObject.Find("Main Camera"));
            SceneManager.LoadScene("Opening", LoadSceneMode.Additive);
            openingButton.SetActive(false);
        }
        else
        {
            openingButton.SetActive(true);
            titleImage.SetActive(true);
            logoImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRankingOpen)
            return;

        if (player.GetButtonDown("Home") && state == TitleState.PressStart)
        {
            state = TitleState.Menu;
            pressStartText.SetActive(false);
            buttons.SetActive(true);
            soundManager.PlaySEOneShot("Decide");
            EventSystem.current.SetSelectedGameObject(startButton);
            startButton.GetComponent<Button>().OnSelect(null);
        }

        if (player.GetButtonDown("Jump"))
        {
            if (state == TitleState.Menu)
            {
                state = TitleState.PressStart;
                pressStartText.SetActive(true);
                buttons.SetActive(false);

            }

            if (state == TitleState.StageSelect)
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
            SceneLoader.LoadScene(GameScenes.GameEasy);
            //SceneManager.LoadScene("StageSelect", LoadSceneMode.Additive);
        }
        else
        {
            titleUI.SetActive(false);
            GameObject.Find("OpeningManager").GetComponent<OpeningManager>().StartOpening();
        }
    }

    public void OnClickRankingButton()
    {
        isRankingOpen = true;
        buttons.SetActive(false);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(0);
    }

    void OnCloseRanking()
    {
        buttons.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);
                startButton.GetComponent<Button>().OnSelect(null);
        isRankingOpen = false;
    }

    public void OnOpeningButtonClicked()
    {
        titleUI.SetActive(false);
        SceneLoader.LoadScene(GameScenes.OpeningBase, additiveLoadScenes: new GameScenes[] { GameScenes.Opening });

    }
}
