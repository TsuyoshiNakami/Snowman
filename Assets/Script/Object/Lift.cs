using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Lift : PointMove {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
		Move (transform.position);
	}





	void OnTriggerStay2D(Collider2D col) {



	}
	void OnTriggerExit2D(Collider2D col) {
		target = null;

	}
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//enum STATE {
//	MOVE,
//	WAIT
//}
//
//public enum MODE {
//	STRAIGHT,
//	CIRCULAR
//}
//
//public class PointMove {
//}
//public class Lift : MonoBehaviour {
//	[SerializeField] GameObject[] checkPoints;
//
//	public float wait = 1;
//	public float speed = 2;
//	public Vector2 vel;
//	public MODE mode = MODE.STRAIGHT;
//	int count = 0;
//	int max;
//
//	STATE state;
//	float time;
//
//	Transform target = null;
//	Vector2 prePos;
//	// Use this for initialization
//	void Start () {
//		state = STATE.MOVE;
//		max = checkPoints.Length;
//		time = Time.fixedTime;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		switch (mode) {
//		case MODE.STRAIGHT:
//			MoveStraight ();
//			break;
//		case MODE.CIRCULAR:
//			MoveCircular ();
//			break;
//		}
//
//		prePos = (Vector2)transform.position;
//	}
//
//	void MoveStraight() {
//		switch (state) {
//		case STATE.MOVE:
//			Vector2 goal = (Vector2)(checkPoints [count + 1].transform.position - transform.position);
//			vel = goal.normalized * speed / 100;
//
//			transform.Translate (vel);
//
//
//			CollisionCheck ();
//			if (Mathf.Abs(checkPoints[count + 1].transform.position.x  - transform.position.x)<=0.1 &&
//				Mathf.Abs(checkPoints[count + 1].transform.position.y - transform.position.y) <=0.1
//			) {
//				if (count + 2 == max) {
//					count = -1;
//				} else {
//					count++;
//				}
//				state = STATE.WAIT;
//				time = Time.fixedTime;
//			}
//			break;
//		case STATE.WAIT:
//			vel = Vector2.zero;
//			if (Time.fixedTime - time >= wait) {
//				state = STATE.MOVE;
//			}
//			break;
//		}
//	}
//
//	void CollisionCheck() {
//		Vector2 a = new Vector2 (2f, 0.5f);
//		bool playerColliderCome = false;
//		Collider2D[] cols = Physics2D.OverlapAreaAll ((Vector2)transform.position - a, (Vector2)transform.position + a + Vector2.up * 0.17f);
//		foreach (Collider2D col in cols) {
//			if (col.CompareTag ("Road")) {
//				continue;
//			}
//			if (col.transform.parent != null) {
//				target = col.transform.parent.transform;
//				if (col.CompareTag ("PlayerBody")) {
//					if (playerColliderCome) {
//						target = null;
//					} else {
//						playerColliderCome = true;
//					}
//				}
//			} else {
//				target = col.transform;
//			}
//			if (target != null) {
//
//				target.position += new Vector3 (transform.position.x - prePos.x, transform.position.y - prePos.y, 0);
//			}
//		}
//	}
//	void MoveCircular() {
//		Quaternion q = transform.rotation;
//		transform.RotateAround (checkPoints[0].transform.position,Vector3.forward, speed);
//		transform.rotation = q;
//		CollisionCheck ();
//	}
//	void OnTriggerStay2D(Collider2D col) {
//
//
//
//	}
//	void OnTriggerExit2D(Collider2D col) {
//		target = null;
//
//	}
//}
