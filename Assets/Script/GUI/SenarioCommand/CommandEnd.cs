using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandEnd : ICommand {
	MessageWindowController_old msgWindow;
	public string Tag {
		get { return"end"; }
	}

	public void Command(Dictionary<string, string> command) {
		msgWindow = GameObject.Find ("MessageWindow").GetComponent<MessageWindowController_old>();
		msgWindow.EndMessage ();
	}
}
