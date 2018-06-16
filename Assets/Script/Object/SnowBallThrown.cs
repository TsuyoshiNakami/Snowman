using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallThrown : MonoBehaviour {

	Rigidbody2D rigid;
	Vector2 ThrownPos;
	public float time = 0.5f;
	Vector2 velo;
	Vector2 dis;
	Collider2D[] col;
	GameObject player;
	public float MaxThrow;
	public float lifeTime = 0.5f;
	public  float currentTime = 0;
	bool grounded = false;
	// Use this for initialization
	void Start () {

		//col = player.GetComponentsInChildren<Collider2D> ();
		//foreach(Collider2D c in col) {
		//	Physics2D.IgnoreCollision (c, transform.GetComponent<Collider2D> ());
		//}
		//StartCoroutine(CollisionControl());
	}

	IEnumerator CollisionControl() {

		yield return new WaitForSeconds (0.2f);

		col = player.GetComponentsInChildren<Collider2D> ();
		foreach(Collider2D c in col) {
			//Physics2D.IgnoreCollision (c, transform.GetComponent<Collider2D> (), false);
		}

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.transform.tag == "Road") {
			grounded = true;
		}
	}
	public void SetMovement(Vector2 destPos) {
		//player = GameObject.Find ("Player");
        MaxThrow = 12;// player.GetComponent<PlayerController> ().MaxThrowPower;
		//Debug.Log (MaxThrow);
		rigid = transform.GetComponent<Rigidbody2D> ();
		ThrownPos = transform.position;
		Vector2 dv = destPos - (Vector2)transform.position;

			Vector2 vec = Vector2.zero;
			vec.x = dv.x / time;
			vec.y = (dv.y + 0.5f*9.8f * time * time) / time;

		if (vec.magnitude > MaxThrow) {
			vec = vec / vec.magnitude * MaxThrow;
		}
		rigid.velocity = vec;
	}


	// Update is called once per frame
	void Update () {
		if (grounded) {
			currentTime += Time.deltaTime;
		}
		if (currentTime >= lifeTime) {
			Destroy (this.gameObject);
		}
	}
}
