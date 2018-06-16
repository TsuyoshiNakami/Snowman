using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandEnd : ICommand {
	MessageWindowController msgWindow;
	public string Tag {
		get { return"end"; }
	}

	public void Command(Dictionary<string, string> command) {
		msgWindow = GameObject.Find ("MessageWindow").GetComponent<MessageWindowController>();
		msgWindow.EndMessage ();
	}
}
