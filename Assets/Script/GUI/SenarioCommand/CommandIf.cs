using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandIf : ICommand {
	MessageController msgController;
	string conditions = null;
	string target = null;
	public string Tag {
		get { return"if"; }
	}

	public void Command(Dictionary<string, string> command) {

		msgController = GameObject.Find ("MessageWindow").GetComponent<MessageController>();
		target = command ["target"];
		conditions = command ["conditions"];
		if(Decision()) {
			msgController.JumpLines (Int32.Parse (command ["jump"]));
		}
	}

	public bool Decision() {
		if (target == "coin") {
			if (PlayerController.coin >= Int32.Parse (conditions)) {
				return true;
			}
			return false;
		}
		return false;
	}
}
