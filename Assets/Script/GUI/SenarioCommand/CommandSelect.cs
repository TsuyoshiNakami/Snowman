using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandSelect : ICommand {
	MessageController msgController;
	public string Tag {
		get { return"select"; }
	}

	public void Command(Dictionary<string, string> command) {

		msgController = GameObject.Find ("MessageWindow").GetComponent<MessageController>();
		msgController.SelectChoice (Int32.Parse(command["yes"]), Int32.Parse(command["no"]));
	}
}
