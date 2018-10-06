using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[System.Serializable]
public class MessageWindowImage
{
    public string displayName;
    public string id;
    public Sprite faceSprite;
}
public class MessageWindowController : MonoBehaviour {

    MessageWindow messageWindow;
    [SerializeField] List<MessageWindowImage> messageWindowImages;
    float timer;
    [SerializeField] GameObject windowObject;
    [SerializeField] Image faceImage;
    [SerializeField] float charTime;
    List<string> messages;
    int messageNum = 0;
    bool allMessageShown = false;
    public bool isShowing = false;

    Subject<Unit> messageFinishedSubject = new Subject<Unit>();
    public IObservable<Unit> OnMessageFinished
    {
        get
        {
            return messageFinishedSubject;
        }
    }

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
            messageFinishedSubject.OnNext(Unit.Default);
            return;
        }
        if (messages[messageNum].IndexOf("@") > -1)
        {
            
            string[] command = messages[messageNum].Split('@')[1].Split(' ');
            if(command[0] == "Face")
            {
                foreach(MessageWindowImage image in messageWindowImages)
                {
                    if(image.id == command[1])
                    {
                        ChangeImage(image.faceSprite);
                    }
                }
            }
            messageNum++;
        }
    }

    void ChangeImage(Sprite sprite)
    {
        faceImage.sprite = sprite;
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
        messageNum = -1;
        ShowNextMessage();
    }

    public void ShowWindow()
    {
        windowObject.SetActive(true);

        messageWindow.SetText("");
        isShowing = true;

        Pauser.Pause();
    }

    public void hideWindow()
    {
        windowObject.SetActive(false);
        isShowing = false;
        Pauser.Resume();
    }
}
