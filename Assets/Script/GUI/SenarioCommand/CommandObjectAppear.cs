using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandObjectAppear: ICommand {
	CommandController cc;
	public string Tag {
		get { return"appear"; }
	}

	public void Command(Dictionary<string, string> command) {
		cc= GameObject.Find ("MessageWindow").GetComponent<CommandController>();
		cc.CCAppearObject (Int32.Parse(command["number"]));
	}
}
