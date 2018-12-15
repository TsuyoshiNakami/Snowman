using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;
using naichilab;
using UniRx;
using Rewired;

public class ResultManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sumText;
    [SerializeField] Button[] buttons;
    [SerializeField] NumberDisplay scoreDisplay;
    [SerializeField] GameObject buttonPanel;
    [Inject]
    PresentManager presentManager;

    Player player;
    bool isRankingOpen;

    // Use this for initialization
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
        buttonPanel.SetActive(false);
        ShowResult();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRankingOpen)
        {
#if Engineer
            if (player.GetButtonDown("Jump"))
#else
            if (Input.GetButtonDown(KeyConfig.Jump))
#endif
            {
                OnCloseRanking();
            }
        }

    }

    public void ShowResult()
    {

        StartCoroutine(ShowResultCoroutine());
    }

    IEnumerator ShowResultCoroutine()
    {
        yield return new WaitForSeconds(2);
        WaitForSeconds wait = new WaitForSeconds(0.02f);
        float t = Time.time;
        while (true)
        {
            if (scoreDisplay != null)
            {
                scoreDisplay.SetNumberImmediately(Random.Range(0, 9999));
            }
            if (Time.time - t > 1.5f) { break; }
            yield return wait;
        }

        while (true)
        {
            if (scoreDisplay != null)
            {
                scoreDisplay.SetNumberImmediately(PresentGameManager.score);
                break;
            }
            yield return wait;
        }

        buttonPanel.SetActive(true);
        SetButtonsInteractive(true);
        InitButtonFocus();
    }

    public void SetButtonsInteractive(bool f)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = f;
        }
    }

    public void InitButtonFocus()
    {
        EventSystem.current.SetSelectedGameObject(buttons[1].gameObject);
    }

    public void OnOpenRanking()
    {

        GameObject.Find("RankingLoader").GetComponent<RankingLoader>()
            .OnCloseRanking
            .First()
            .Subscribe(_ =>
            {
                OnCloseRanking();
            });
        SetButtonsInteractive(false);
        isRankingOpen = true;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(PresentGameManager.score);

        //SceneManager.LoadScene("RankingAdditive", LoadSceneMode.Additive);
    }

    public void OnCloseRanking()
    {
        SetButtonsInteractive(true);
        //SceneManager.UnloadSceneAsync("RankingAdditive");
        isRankingOpen = false;
        InitButtonFocus();
    }
}
