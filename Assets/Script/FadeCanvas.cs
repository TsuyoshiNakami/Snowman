using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour {

	[SerializeField] Fade fade = null;
	// Use this for initialization
	public void OnClick() {
		fade.FadeOut (2);
	}
}
