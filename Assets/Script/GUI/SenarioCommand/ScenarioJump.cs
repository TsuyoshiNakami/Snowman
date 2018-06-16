using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class ScenarioJump : ICommand {
	MessageController msgController;
	public string Tag {
		get { return"jump"; }
	}
	
	public void Command(Dictionary<string, string> command) {
		
		msgController = GameObject.Find ("MessageWindow").GetComponent<MessageController>();
		msgController.JumpLines (Int32.Parse(command["number"]));
	}
}
