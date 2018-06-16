using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandStay: ICommand {
	CommandController cc;
	public string Tag {
		get { return"stay"; }
	}

	public void Command(Dictionary<string, string> command) {
		GameObject.Find ("MesssageWindow").GetComponent<MessageController> ().m_stop = true;
		if (command ["command"] == "resume") {
		}

	}
}
