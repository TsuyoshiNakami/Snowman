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
    Opening
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
    [SerializeField] GameObject snowParticle;

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

        if (!ES3.KeyExists("Tutorial"))
        {
            snowParticle.SetActive(false);
            Destroy(GameObject.Find("Main Camera"));
            SceneManager.LoadScene("Opening", LoadSceneMode.Additive);
            openingButton.SetActive(false);
        }
        else
        {
            soundManager.PlayBGM("Title");
            snowParticle.SetActive(true);
            openingButton.SetActive(true);
            titleImage.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRankingOpen)
            return;

        if (state == TitleState.PressStart) {
#if engineer
        if (player.GetButtonDown("Home") || player.GetButtonDown("Fire"))
#else
            if (Input.GetButtonDown(KeyConfig.Home)  )
#endif
            {
                state = TitleState.Menu;
                pressStartText.SetActive(false);
                buttons.SetActive(true);
                soundManager.PlaySEOneShot("Decide");
                EventSystem.current.SetSelectedGameObject(startButton);
                startButton.GetComponent<Button>().OnSelect(null);
            }
        }

#if engineer
            if (player.GetButtonDown("Jump"))
#else
        if (Input.GetButtonDown(KeyConfig.Jump))
#endif
        {
            if (state == TitleState.Menu)
            {
                soundManager.PlaySEOneShot("Cancel");
                state = TitleState.PressStart;
                pressStartText.SetActive(true);
                buttons.SetActive(false);

            }

            //if (state == TitleState.StageSelect)
            //{
            //    state = TitleState.Menu;
            //    buttons.SetActive(true);
            //    EventSystem.current.SetSelectedGameObject(startButton);
            //    startButton.GetComponent<Button>().OnSelect(null);
            //    SceneManager.UnloadSceneAsync("StageSelect");
            //}
        }
    }


    bool gameStarted;
    public void OnGameStartButtonClicked()
    {
        if(gameStarted)
        {
            return;
        }
        gameStarted = true;
        if (ES3.KeyExists("Tutorial"))
        {
        soundManager.PlaySEOneShot("DecideBig");
            soundManager.FadeOut(1);
            buttons.SetActive(false);
            SceneLoader.LoadScene(GameScenes.GameEasy);
            //SceneManager.LoadScene("StageSelect", LoadSceneMode.Additive);
        }
        else
        {
        soundManager.PlaySEOneShot("Decide");
            state = TitleState.Opening;
            titleUI.SetActive(false);
            GameObject.Find("OpeningManager").GetComponent<OpeningManager>().StartOpening();
        }
    }

    public void OnClickRankingButton()
    {
                soundManager.PlaySEOneShot("Decide");
        GameObject.Find("RankingLoader").GetComponent<RankingLoader>().OnCloseRanking
       .First()
       .Subscribe(_ =>
           {
               OnCloseRanking();
           });
        isRankingOpen = true;
        buttons.SetActive(false);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(0);
    }

    void OnCloseRanking()
    {
                soundManager.PlaySEOneShot("Cancel");
        buttons.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);
        startButton.GetComponent<Button>().OnSelect(null);
        isRankingOpen = false;
    }

    public void OnOpeningButtonClicked()
    {
            soundManager.FadeOut(1);
        soundManager.PlaySEOneShot("DecideBig");
        titleUI.SetActive(false);
        SceneLoader.LoadScene(GameScenes.OpeningBase, additiveLoadScenes: new GameScenes[] { GameScenes.Opening });

    }
}
