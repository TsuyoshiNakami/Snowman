using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class CommandController : MonoBehaviour {
	[SerializeField]private appearObjectInfo[] appearObjectList;
	[SerializeField]private GameObject[] destroyObjectList;

	private readonly List<ICommand> CommandList = new List<ICommand> () {
	//	new CommandUpdateImage(),
		new CommandEnd(),
		new ScenarioJump(),
		new CommandSelect(),
		new CommandObjectAppear(),
		new CommandObjectDestroy(),
		new CommandPlayBGM(),
		new CommandIf(),
		new CommandPauser(),
		new CommandStay(),
	};

	private List<IPreCommand> preCommandList = new List<IPreCommand>();

	public void PreloadCommand(string line) {
		var dic = CommandAnalytics (line);
		foreach (var command in preCommandList)
			if (command.Tag == dic ["tag"])
				command.PreCommand (dic);
	}

	public bool loadCommand(string line) {

		var dic = CommandAnalytics (line);
		foreach (var command in CommandList) {
			if (command.Tag == dic ["tag"]) {
				command.Command (dic);
				return true;
			}
		}
		return false;
	}
		

	private Dictionary<string, string> CommandAnalytics(string line) {
		Dictionary<string, string> command = new Dictionary<string, string> ();

		var tag = Regex.Match (line, "@(\\S+)\\s");
		command.Add ("tag", tag.Groups [1].ToString ());

		Regex regex = new Regex ("(\\S+)=(\\S+)");
		var matches = regex.Matches (line);

		foreach (Match match in matches) {
			command.Add (match.Groups [1].ToString (), match.Groups [2].ToString ());
		}
		return command;
	}
	#region UNITY_CALLBACK
	void Awake() {
		foreach (var command in CommandList) {
			if (command is IPreCommand) {
				preCommandList.Add ((IPreCommand)command);
			}
		}
	}
	#endregion
	public void CCDestroyObject() {
		if (destroyObjectList.Length > 0) {
			foreach (GameObject destroyObject in destroyObjectList) {
				Destroy (destroyObject, 1.0f);
			}
		}
	}
	public void CCDestroyObject(int i) {
		//Debug.Log ("destroy");
		if(destroyObjectList[i] != null) {
			Destroy (destroyObjectList[i], 0f);
		}
	}
	public void CCAppearObject() {
		if (appearObjectList.Length > 0) {
			foreach (appearObjectInfo appearObject in appearObjectList) {
				appearObject.appearObj.SetActive (true);
				//Instantiate (appearObject.appearObj, appearObject.position, transform.rotation);
			}
		}
	}
	public void CCAppearObject(int i) {
		if(appearObjectList[i] != null) {
			appearObjectList[i].appearObj.SetActive (true);
		}
	}

}
