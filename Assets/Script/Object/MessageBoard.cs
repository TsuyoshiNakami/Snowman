using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoard : Item {



	protected override void ActStay ()
	{

		Destroy (gameObject);
	}
}
