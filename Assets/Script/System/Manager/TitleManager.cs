using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Tsuyomi.Yukihuru.Scripts.Utilities;
using naichilab;
using UniRx;
#if engineer
using Rewired;
#endif


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
    [SerializeField] GameObject openingButton;

    bool isRankingOpen;
    SoundManager soundManager;
#if engineer
    Player player;
#endif

    // Use this for initialization
    void Start()
    {

#if engineer
        player = ReInput.players.GetPlayer(0);

#endif
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRankingOpen)
            return;
#if engineer
        if (player.GetButtonDown("Home") && state == TitleState.PressStart)
#else
        if (Input.GetButtonDown(KeyConfig.Home) && state == TitleState.PressStart)
#endif
        {
            state = TitleState.Menu;
            pressStartText.SetActive(false);
            buttons.SetActive(true);
            soundManager.PlaySEOneShot("Decide");
            EventSystem.current.SetSelectedGameObject(startButton);
            startButton.GetComponent<Button>().OnSelect(null);
        }

#if engineer
            if (player.GetButtonDown("Jump"))
#else
        if (Input.GetButtonDown(KeyConfig.Jump))
#endif
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
