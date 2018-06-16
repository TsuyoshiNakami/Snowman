using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MessageWindowController : MonoBehaviour {




	public static string msgWindowInfo = "";
	string currentTextName = "";
	public bool isMsgWindowActive = false;
	public string msgVoice = "";
	public Speak dogSpeak;
	public Speak boarSpeak;

	public bool selectedChoice = true;

	[SerializeField][Range(0.001f, 0.3f)]
	public float intervalForCharacterDisplay = 0.05f;

	public string currentText = string.Empty;
	private float timeUntilDisplay = 0;
	private float timeElapsed = 1;

	private int lastUpdateCharacter = -1;
	public Image uiPanel;
	public Text uiTextYes;
	public Text uiTextNo;
	public Image uiCursor;
	public Text uiText;
	public MessageController messageController;

	public bool IsCompleteDisplayText 
	{
		get { return  Time.time > timeElapsed + timeUntilDisplay; }
	}
	public void ForceCompleteDisplayText ()
	{
		timeUntilDisplay = 0;
	}

	public void SetNextLine(string text)
	{

		//uiTextYes.gameObject.SetActive (false);
		//uiTextNo.gameObject.SetActive (false);
		//uiCursor.gameObject.SetActive (false);
		//selectedChoice = true;
		if (text != "") {
			currentText = text;

			if (messageController.m_stop) {
				intervalForCharacterDisplay = 0.1f;
			}
			timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
			timeElapsed = Time.time;

			lastUpdateCharacter = -1;

		}

	}
	public void EndMessage() {
		VoiceStop ();
		//currentLine = 0;
		msgWindowInfo = currentTextName + "_End";
		Pauser.Resume ();
		SetActive (false);
	}

	void Start() {
		messageController = GetComponent<MessageController> ();
	}
	public void OutputMessage(string msgName) {

		selectedChoice = true;
		//scenarios = str;
		messageController.UpdateLines(msgName);
		currentTextName = msgName;
		msgWindowInfo = msgName;

		messageController.RequestNextLine ();
		//currentLine = 0;
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;
		lastUpdateCharacter = -1;
		SetActive(true);
	}
	public void SetActive(bool f) {
		if (f) {

		}
	//	uiText.text = scenarios [currentLine];
		isMsgWindowActive = f;
		uiText.gameObject.SetActive(f);
		uiPanel.gameObject.SetActive(f);
		if (!f) {
			uiTextYes.gameObject.SetActive (f);
			uiTextNo.gameObject.SetActive (f);
			uiCursor.gameObject.SetActive (f);
		}
	}

	public void SelectMessage(bool f) {

		uiTextYes.gameObject.SetActive (f);
		uiTextNo.gameObject.SetActive (f);
		uiCursor.gameObject.SetActive (f);
	}

	void CursorMove() {
		Transform cursor = transform.Find("Cursor");
		if (selectedChoice) {
			cursor.position = new Vector3 (240f, cursor.position.y, 0);
		} else {
			cursor.position = new Vector3 (580f, cursor.position.y, 0);
		}
	}
	void Update () 
	{
		if (!isMsgWindowActive) {
			uiText.gameObject.SetActive(false);
			uiPanel.gameObject.SetActive(false);
			uiTextYes.gameObject.SetActive (false);
			uiTextNo.gameObject.SetActive (false);
			uiCursor.gameObject.SetActive (false);
			return;
		}
		if (uiCursor.gameObject.active) {
			CursorMove ();
			if(Input.GetButtonDown("Horizontal")) {
				selectedChoice = selectedChoice ? false : true;
			}
		}

		if (messageController.hiSpeed) {
			timeUntilDisplay = 0.5f;
		}
		int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

		if (displayCharacterCount != lastUpdateCharacter) {
			uiText.text = currentText.Substring (0, displayCharacterCount);
			lastUpdateCharacter = displayCharacterCount;
		}
	}

	public static void clearInfo() {
		msgWindowInfo = "";
	}
	public void Appear() {
		
	}

	public void VoiceCheck() {
		string msg = msgWindowInfo;

		if (msg == "Msg_LiftCharacter" || msg == "Msg_LiftCharacter_After") {
			dogSpeak.Speaking ();
		} else if (msg == "Msg_PreBoss" || msg == "Msg_BossDefeated") {
			boarSpeak.Speaking ();
		} 
	}

	public void VoiceStop() {
	 {
			dogSpeak.StopSpeaking ();
			boarSpeak.StopSpeaking ();
		}
	}
}
