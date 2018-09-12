using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MessageController : MonoBehaviour {
	string LoadFileName;

	public string[] scenarios;
	private int currentLine = 0;
	private bool m_isCallPreLoad = false;
	public bool m_stop = false;
	public bool hiSpeed = false;
	private MessageWindowController_old msgWindow;
	//private CommandController commandController;
	int yesGoto = 0;
	int noGoto = 0;


	float autoTime = 0;
	float autoTimer = 2;
	public void RequestNextLine() {
		msgWindow.VoiceCheck ();

		var currentText = scenarios [currentLine];
	//	Debug.Log ("RequestNextLine1: " + currentText);
	//	Debug.Log ("RequestNextLine2: " + currentLine);
//		Debug.Log (currentText);
		msgWindow.SetNextLine (CommandProcess(currentText));

		currentLine++;
		m_isCallPreLoad = false;
	}

	public void UpdateLines(string fileName) {

		var scenarioText = Resources.Load<TextAsset> ("Scenario/" + fileName);

		if (scenarioText == null) {
			Debug.LogError ("シナリオファイルが見つからない");
			enabled = false;
			return;
		}
	//	Pauser_old.Pause ();
		scenarios = scenarioText.text.Split (new string[]{ "@br" }, System.StringSplitOptions.None);
		currentLine = 0;
		//Debug.Log ("UpdateLines" + currentLine);
		Resources.UnloadAsset (scenarioText);
	}

	public void JumpLines(int number) {

		currentLine = 0;

		while(currentLine < scenarios.Length) {
			currentLine++;
			var commentCharacterCount = scenarios [currentLine].IndexOf ("@" + number);
			if (commentCharacterCount >= 0) {
				return ;
			}

		}
		return;
	}

	public void SelectChoice(int yes, int no) {
		yesGoto = yes;
		noGoto = no;

		msgWindow.SelectMessage (true);
	}

	public void Choose() {
		msgWindow.SelectMessage (false);
		if(msgWindow.selectedChoice) {
			Debug.Log (yesGoto);
			JumpLines (yesGoto);
			currentLine++;
			RequestNextLine ();
		} else {
			JumpLines (noGoto);
			currentLine++;
			RequestNextLine ();
		}
	}
	private string CommandProcess(string line) {
		var lineReader = new StringReader (line);
		var lineBuilder = new StringBuilder ();
		var text = string.Empty;
		while((text = lineReader.ReadLine()) != null) {
			var commentCharacterCount = text.IndexOf ("//");
			if(commentCharacterCount != -1) {
				text = text.Substring (0, commentCharacterCount);
			}
			text = text.Replace ("br", "\n");
			if (text.IndexOf ("stay") >= 0) {
				text = text.Replace ("stay", "");
				m_stop = true;
			}
			if(! string.IsNullOrEmpty(text)) {
				if (text [0] == '@' ){ //&& commandController.loadCommand (text)) {
					if (text.IndexOf( "@jump") >= 0) {
						//Debug.Log("CommandProcess" + scenarios [1+currentLine]);
						return scenarios [++currentLine];
					}

					continue;
				}

				lineBuilder.Append(text);
			}
		}
		return lineBuilder.ToString();
	}

			#region UNITY_CALLBACK
			void Start() {
				msgWindow = GetComponent<MessageWindowController_old>();
				//commandController = GetComponent<CommandController>();
	
			}
	void Update() {
		if (!msgWindow.isMsgWindowActive)
			return;
		if (m_stop) {

			if (hiSpeed) {
				autoTimer = 0;
			} else {
				autoTimer = 2;
			}
			if (msgWindow.IsCompleteDisplayText) {
				if (currentLine < scenarios.Length) {

					autoTime += Time.deltaTime;
					if (hiSpeed) {
						autoTime = autoTimer;
					}
					if (autoTimer <= autoTime) {
						autoTime = 0;
				//		autoTimer = 0;

						RequestNextLine ();
					}
				} else {
					autoTime += Time.deltaTime;
					if (hiSpeed) {
						autoTime = autoTimer;
					}
					if (autoTimer <= autoTime) {
						autoTime = 0;
						autoTimer = 0;

						msgWindow.EndMessage ();
					}
				}
			}
			return;
		}
		if(msgWindow.IsCompleteDisplayText) {
			msgWindow.VoiceStop ();
			if (currentLine < scenarios.Length) {
				if (!m_isCallPreLoad) {
					//commandController.PreloadCommand (scenarios [currentLine]);
					m_isCallPreLoad = true;
				}

				if (Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Jump")) {
					if (msgWindow.uiTextYes.isActiveAndEnabled) {
						Choose ();
					} else {
						RequestNextLine ();
					}
				}
			} else {
				if (Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Jump")) {
					msgWindow.EndMessage ();
				}
			}
		}else {
			if(Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Jump")) {
									msgWindow.ForceCompleteDisplayText();
			}
		}
	}
								#endregion

}
