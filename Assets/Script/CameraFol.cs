using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFol : MonoBehaviour {
	GameObject player;
	PlayerController pc;
	float pY;
	Vector2 target;
	public float smoothingY = 2f;
	public float offsetY = 2f;
	public bool followPlayer = true;
	public Vector2 lerpScroll;
	float time = 0;
	float timer = 0;
	// Use this for initialization
	void Start () {
		lerpScroll = Vector2.zero;
		player = GameObject.Find ("Player");
		pc = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (lerpScroll != Vector2.zero) {
			timer = 2;
		}
		if(timer > 0) {
			time += Time.fixedDeltaTime;
			transform.position = new Vector3 (transform.position.x + lerpScroll.x,transform.position.y + lerpScroll.y, -10);
			if (time >= timer) {
				timer = 0;
				time = 0;
				lerpScroll = Vector2.zero;
			}
		}
		if (pc.activeSts == false || !followPlayer) {
			return;
		}
		target = player.transform.position;
		pY = Mathf.Lerp(transform.position.y,pc.groundY,smoothingY);
		transform.position = new Vector3 (player.transform.position.x, pY + offsetY, -10);
	}

	public void Scroll(Vector2 move) {
		//	 = new Vector3 (pos.x + move.x, move.y + pos.y, 10);
		transform.position += (Vector3)move;
	}
}
