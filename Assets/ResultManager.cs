using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class ResultManager : MonoBehaviour
{
    [SerializeField] GameObject resultWindow;

    [SerializeField] GameObject resultElement;
    [SerializeField] Transform resultTransform;
    [SerializeField] TextMeshProUGUI sumText;
    [SerializeField] Button[] buttons;
    [SerializeField] NumberDisplay scoreDisplay;
    [SerializeField] GameObject buttonPanel;
    [Inject]
    PresentManager presentManager;


    // Use this for initialization
    void Start()
    {
        buttonPanel.SetActive(false);
        ShowResult();
    }

    // Update is called once per frame
    void Update()
    {

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
    }
