using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningChara : MonoBehaviour {
	Animator anime;
	bool isWalking = false;
	float time = 0;
	// Use this for initialization
	void Start () {
		anime = GetComponent<Animator> ();
	}
	public void Shake() {
		iTween.ShakePosition(gameObject,iTween.Hash ("x", 0.1f, "y", 0.0f, "time", 2f));
	}
	public void Walk() {
		anime.SetTrigger ("Walk");
		isWalking = true;
	}
	// Update is called once per frame
	void Update () {
		if (isWalking) {
			transform.Translate (new Vector3 (0.05f, 0));
			time += Time.deltaTime;
		}
		if (time > 5) {
			Destroy (gameObject);
		}
	}
}
