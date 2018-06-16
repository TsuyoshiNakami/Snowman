using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speak : MonoBehaviour {
	AudioSource[] source;
	public float pitchMin = 0.8f;
	public float pitchMax = 1.2f;
	public float waitForWord = 0.25f;
	bool isStarted = false;
	// Use this for initialization
	void Start () {
		source = GetComponents<AudioSource> ();
	}


	public void Speaking() {
		if (!isStarted) {
			StartCoroutine ("Repeat");
			isStarted = true;
		}
	}

	IEnumerator Repeat() {

			int sourceNum = Random.Range (0, source.Length);
			source[sourceNum].pitch = Random.Range (pitchMin, pitchMax);
			source[sourceNum].PlayOneShot (source[sourceNum].clip);
			yield return new WaitForSeconds (waitForWord);
		StartCoroutine ("Repeat");
	}

	public void StopSpeaking() {
		isStarted = false;
		StopCoroutine ("Repeat");
	}
}
