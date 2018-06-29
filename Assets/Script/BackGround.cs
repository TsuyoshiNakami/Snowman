using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {
	Transform trans;
	public Vector2 pos;
	public bool followPlayer = true;
	SpriteRenderer sprite;
	public float fadeOutTime = 2;
	float time = 0;
	public bool stageBG = false;
	public bool lastNightSky = false;
	public bool normalSky = false;
	// Use this for initialization
	void Awake () {
		sprite = GetComponent < SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (lastNightSky) {
			float a = 0;
			if (Camera.main.transform.position.x > 278) {
				a = (Camera.main.transform.position.x - 278) / 14;
			}
			Color color = sprite.color;
			color.a = a;
			sprite.color = color;

		}

		if (normalSky) {
			float b = 1;
			float g = 1;
			float r = 1;
			if (Camera.main.transform.position.x > 222) {
				b = 1 - (Camera.main.transform.position.x - 222) / 30;
				g = 1 - (Camera.main.transform.position.x - 222) / 30 * 230 / 255;
				g = 1 - (Camera.main.transform.position.x - 222) / 30 * 80 / 255;
			}
			Color color = sprite.color;
			color.b = b;
			color.g = g;
			color.r = r;
			sprite.color = color;

		}
		if (followPlayer) {
			transform.position = new Vector3 (pos.x + Camera.main.transform.position.x, Camera.main.transform.position.y + pos.y, 10);
		} else {
			if(!stageBG)transform.position = new Vector3 (pos.x,pos.y, 10);
		}
	}

	public void Scroll(Vector2 move) {
	//	transform.position = new Vector3 (pos.x + move.x, move.y + pos.y, 10);
		pos += move;
	}

	public float FadeOut() {

		time += Time.deltaTime;
		Color color = sprite.color;
		color.a = (-255f / fadeOutTime * time + 255f) / 255f - 1/255f;
		sprite.color = color;
		foreach (Transform child in transform) {
			child.GetComponent<SpriteRenderer> ().color = color;
		}
		//color.a -= 0.01f;

		if (color.a < 0) {
			return -1;
		}

		return color.a;
	}
}
