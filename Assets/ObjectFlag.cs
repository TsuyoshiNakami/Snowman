using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectFlag : MonoBehaviour {
	enum FLAGS {
		BOSSDEFEATED,
		HAVEPLAYED,
		MIDDLEBOSSDEFEATED,
	};
	enum CHECKTIMING {
		START,
		UPDATE,
	};
	[SerializeField]FLAGS Flag;
	[SerializeField]CHECKTIMING Timing = CHECKTIMING.UPDATE;
	[SerializeField]GameObject[] appearObj;
	// Use this for initialization
	void Start () {
		if (Timing == CHECKTIMING.START) {
			switch (Flag) {
			case FLAGS.BOSSDEFEATED:
				if (GameManager.playerData.bossDefeated) {
					foreach (GameObject obj in appearObj) {
						obj.SetActive (true);
					}
					Destroy (this.gameObject);
				}
				break;
			case FLAGS.HAVEPLAYED:
				if (!GameManager.isFirstPlay) {
					Destroy (this.gameObject);
				}
				break;
			case FLAGS.MIDDLEBOSSDEFEATED:
				if (GameManager.playerData.middleBossDefeated) {
					Destroy (this.gameObject);
				}
				break;
				Destroy (this);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Timing == CHECKTIMING.UPDATE) {
			switch (Flag) {
			case FLAGS.BOSSDEFEATED:
				if (GameManager.playerData.bossDefeated) {
					Destroy (this.gameObject);
				}
				break;
			case FLAGS.HAVEPLAYED:
				if (!GameManager.isFirstPlay) {
					Destroy (this.gameObject);
				}
				break;
			case FLAGS.MIDDLEBOSSDEFEATED:
				if (GameManager.playerData.middleBossDefeated) {
					Destroy (this.gameObject);
				}
				break;
				Destroy (this);
			}
		}
	}
}
