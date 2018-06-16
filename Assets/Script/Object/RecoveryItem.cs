using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryItem : Item {

	protected override void Awake ()
	{
		base.Awake ();
	}
	protected override void ActEnter() {

		pc.Recovery(1);

		Destroy (gameObject);
	}
}
