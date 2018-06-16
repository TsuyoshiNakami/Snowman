using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandPauser: ICommand {
	CommandController cc;
	public string Tag {
		get { return"pauser"; }
	}

	public void Command(Dictionary<string, string> command) {
		if (command ["command"] == "resume") {
			Pauser.Resume (command ["name"]);
		}

	}
}
