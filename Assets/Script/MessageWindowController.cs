using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageWindowController : MonoBehaviour {

    MessageWindow messageWindow;
    float timer;
    [SerializeField] GameObject windowObject;
    [SerializeField] float charTime;
    List<string> messages;
    int messageNum = 0;
    bool allMessageShown = false;
    public bool isShowing = false;

	// Use this for initialization
	void Awake () {

        messageWindow = GetComponent<MessageWindow>();
        messages = new List<string>();
        hideWindow();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isShowing)
        {
            return;
        }

        UpdateText();
        if(Input.GetButtonDown(KeyConfig.NextMessage))
        {
            OnMessageButtonDown();
        }
	}

    void OnMessageButtonDown()
    {
        if (allMessageShown)
        {
            ShowNextMessage();
        }
        else
        {
            ViewAllMessage();
        }
    }

    void ShowNextMessage()
    {
        messageNum++;
        timer = 0;
        allMessageShown = false;
        if (messages.Count - 1 < messageNum)
        {
            hideWindow();
        }
    }

    void ViewAllMessage()
    {
        timer = charTime * messages[messageNum].Length;
    }

    public void UpdateText()
    {

        timer += Time.deltaTime;
        int charNum = 999;
        if (charTime > 0)
        {
            charNum = (int)(timer / charTime);
        }

        if(charNum > messages[messageNum].Length)
        {
            allMessageShown = true;
            charNum = messages[messageNum].Length;
        }

        string newMessage = messages[messageNum].Substring(0, charNum);
        messageWindow.SetText(newMessage);
    }

    public void StartMessage(List<string> messages)
    {
        ShowWindow();
        this.messages = messages;
    }

    public void ShowWindow()
    {
        windowObject.SetActive(true);

        messageWindow.SetText("");
        isShowing = true;
        messageNum = 0;
        timer = 0;
        Pauser.Pause();
    }

    public void hideWindow()
    {
        windowObject.SetActive(false);
        isShowing = false;
        Pauser.Resume();
    }
}
