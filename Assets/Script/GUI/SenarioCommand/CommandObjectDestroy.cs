using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandObjectDestroy: ICommand {
	CommandController cc;
	public string Tag {
		get { return"destroy"; }
	}

	public void Command(Dictionary<string, string> command) {
		cc= GameObject.Find ("MessageWindow").GetComponent<CommandController>();
		cc.CCDestroyObject (Int32.Parse(command["number"]));
	}
}
