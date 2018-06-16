using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Coin : Item {
	[SerializeField]int number = 0;
	protected override void Awake ()
	{

		base.Awake ();
	}

	void Update() {
	//	iTween.ShakeRotation (this.gameObject, iTween.Hash ("z", 10, "time", 1.0f));
		if (GameManager.getCoins [number]) {
			Destroy (this.gameObject);
		}
	}
	protected override void ActEnter() {
		if (!GameManager.firstCoinGet) {
			GameManager.firstCoinGet = true;
		}
		pc.CoinGet();
		GameManager.getCoins [number] = true;
		Destroy (gameObject);
	}

}
