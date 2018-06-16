using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENEMYBALLTYPE {
	STRAIGHT,
	RIGID
}

public class EnemyBall : MonoBehaviour {
	public Transform ball;
	public float speed = 0.1f;
	public ENEMYBALLTYPE type = ENEMYBALLTYPE.STRAIGHT;


	public EnemyBall(Transform eb) {
		ball = eb;
		if (type == ENEMYBALLTYPE.RIGID) {
			gameObject.AddComponent<Rigidbody2D> ();
		}
	}
}
public class BallEnemyThrow : MonoBehaviour {
	 public ENEMYBALLTYPE type = ENEMYBALLTYPE.STRAIGHT;
	 public float speed = 2;
	 public float lifeTime = 3;
	public float fallTime = 2;
	float time;
	Rigidbody2D rigid;
	float targetX;
	// Use this for initialization
	void Start () {
		time = Time.fixedTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.fixedTime - time >= lifeTime) {
			Destroy (gameObject);
		}
		switch (type) {
		case ENEMYBALLTYPE.STRAIGHT:
			transform.Translate (new Vector2 (speed, 0));
			break;
		case ENEMYBALLTYPE.RIGID:

			break;
		}
	}

	public void rigidCalc() {
		rigid = transform.GetComponent<Rigidbody2D> ();
		targetX = GameObject.Find ("Player").transform.position.x;

		Vector2 dv = new Vector2 (targetX - transform.position.x, 0);// - (Vector2)transform.position;

		Vector2 vec = Vector2.zero;
		vec.x = dv.x / fallTime;
		vec.y = (dv.y + 0.5f*9.8f * fallTime * fallTime) / fallTime;

		//			if (vec.magnitude > MaxThrow) {
		//				vec = vec / vec.magnitude * MaxThrow;
		//			}
		rigid.velocity = vec;
	}
}
