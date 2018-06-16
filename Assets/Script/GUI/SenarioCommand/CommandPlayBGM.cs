using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CommandPlayBGM: ICommand {
	SoundManager soundManager;
	public string Tag {
		get { return"BGM"; }
	}

	public void Command(Dictionary<string, string> command) {
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager>();

		if (!command.ContainsKey("action")){// || command ["action"] == "play") {
			if (command.ContainsKey ("loop")) {
				soundManager.PlayBGM (command ["name"], false);
			} else {
				soundManager.PlayBGM (command ["name"]);
			}
		} else if (command ["action"] == "fadeout") {
			soundManager.FadeOut (Int32.Parse (command ["time"]));
		}
	}
}
