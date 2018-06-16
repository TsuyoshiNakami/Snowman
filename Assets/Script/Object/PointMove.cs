using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum POINTMOVESTATE {
	MOVE,
	WAIT
}

public enum MODE {
	STRAIGHT,
	CIRCULAR
}
public class PointMove : MonoBehaviour {
	[SerializeField] public GameObject[] checkPoints;
	[SerializeField] public float wait = 1;
	[SerializeField] public float speed = 2;
	[SerializeField] public Vector2 vel;
	[SerializeField] public MODE mode = MODE.STRAIGHT;
	[SerializeField] public int count = 0;
	[SerializeField] public int max;

	[SerializeField] POINTMOVESTATE state;
	[SerializeField] public float time;
	[SerializeField] bool doesCollisionCheck = true;
	public bool StartWithPlayer = false;
	protected Transform target = null;
	protected Vector2 prePos;
	bool playerColliderCome = false;
	bool arrivedAtGoal = false;

	float backToStartEndTime = 0;
	void Awake() {
		state = POINTMOVESTATE.MOVE;
		max = checkPoints.Length;
		time = Time.fixedTime;
		prePos = (Vector2)transform.position;
	}

	public void Move(Vector3 vec) {
		if (arrivedAtGoal && StartWithPlayer) {
			return;
		}
		CollisionCheck ();
		if (checkPoints.Length == 0) {
			return;
		}

		if (StartWithPlayer) {

			if (!playerColliderCome) {

				if (backToStartEndTime == 0) {
				//	Debug.Log ("乗ってない");

					BackToStart ();
					prePos = (Vector2)vec;
					return;

				} else {
					//Debug.Log ("一度乗ってから降りた");
					backToStartEndTime += Time.deltaTime;
					if (1.5f < backToStartEndTime && backToStartEndTime < 2f) {
						
						return;
					} else if (backToStartEndTime >= 2f) {
						backToStartEndTime = 0;
					}

				}
	
			} else {
				//Debug.Log ("乗っている");

				backToStartEndTime = 0.1f;
			}


		}

		switch (mode) {
		case MODE.STRAIGHT:
			MoveStraight ();
			break;
		case MODE.CIRCULAR:
			MoveCircular ();
			break;
		}

		prePos = (Vector2)vec;
	}

	void BackToStart() {

		switch (mode) {
		case MODE.STRAIGHT:
			Vector2 goal = (Vector2)(checkPoints [0].transform.position - transform.position);
			goal = goal.normalized;
				if (Vector3.Distance (transform.position, checkPoints [0].transform.position) > goal.magnitude * speed / 50) {
				//goal = (Vector2)(checkPoints [0].transform.position - transform.position);
				vel = goal * speed / 50;
				transform.Translate (vel);
			} else {

				backToStartEndTime = 0;
			}
			break;
		}	

	}
	void CollisionCheck() {
		if (!doesCollisionCheck) {
			return;
		}
		Vector2 a = new Vector2 (2f * transform.localScale.x, 0.5f);
		playerColliderCome = false;
		Collider2D[] cols = Physics2D.OverlapAreaAll ((Vector2)transform.position - a, (Vector2)transform.position + a + Vector2.up * 0.17f);
		foreach (Collider2D col in cols) {
			if (col.CompareTag ("Road")) {
				continue;
			}
			if (col.transform.parent != null) {
				target = col.transform.parent.transform;
				if (col.CompareTag ("PlayerBody")) {
					if (playerColliderCome) {
						target = null;
					} else {
						playerColliderCome = true;
					}
				}
			} else {
				target = col.transform;
			}
			if (target != null) {
				//Debug.Log (target + ":移動中");
				target.position += new Vector3 (transform.position.x - prePos.x, transform.position.y - prePos.y, 0);
			}
		}
	}


	void MoveCircular() {
		Quaternion q = transform.rotation;
		transform.RotateAround (checkPoints[0].transform.position,Vector3.forward, speed);
		transform.rotation = q;

	}
	void MoveStraight() {

		switch (state) {
		case POINTMOVESTATE.MOVE:
			Vector2 goal = (Vector2)(checkPoints [count + 1].transform.position - transform.position);
			vel = goal.normalized * speed / 100;

			transform.Translate (vel);
			if (Mathf.Abs(checkPoints[count + 1].transform.position.x  - transform.position.x)<=0.1 &&
				Mathf.Abs(checkPoints[count + 1].transform.position.y - transform.position.y) <=0.1
			) {
				arrivedAtGoal = true;

					if (count + 2 == max) {
						count = -1;
					} else {
						count++;
					}
					state = POINTMOVESTATE.WAIT;
					time = Time.fixedTime;

			}

	
			break;
		case POINTMOVESTATE.WAIT:
			vel = Vector2.zero;
			if (Time.fixedTime - time >= wait) {
				state = POINTMOVESTATE.MOVE;
			}
			break;
		}
	}
		
}
