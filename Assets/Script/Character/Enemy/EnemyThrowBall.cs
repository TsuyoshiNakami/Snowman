using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyThrowBall : EnemyMain {
	TURN turn = TURN.WAIT;
	public float throwInterval = 1;
	public float ballSpeed = 0.1f;
	EnemyBall ball;
	public Transform enemyBall;
	public bool isMiddleBoss = false;
	[SerializeField] float hp = 1;
	// Use this for initialization
	void Start () {
		enemyCtrl.setHP (hp, hp);
		IsAnime = false;
		ball = new EnemyBall(enemyBall);
		ball.speed = ballSpeed;
		enemyCtrl.dir = 1;
	}



	public override void FixedUpdateAI() {

		if (!enemyCtrl.activeSts) {
			turn = TURN.DEAD;
			return;
		}
		if (transform.position.y < -7) {
			GameManager.playerData.middleBossDefeated = true;
		}
		enemyCtrl.Lookup (player, 1);
		switch(state) {
		case ENEMYSTS.SELECT:

			if (turn == TURN.WAIT) {
				SetAIState (ENEMYSTS.WAIT, throwInterval);
			} else {
				SetAIState (ENEMYSTS.ATTACK, 1);
			}
			enemyCtrl.Move (0f);
			break;
		case ENEMYSTS.WAIT:
			turn = TURN.ATTACK;

			enemyCtrl.Move (0.0f);

			break;

		case ENEMYSTS.ATTACK:
			if (!enemyCtrl.cameraRendered) {
				break;
			}

			enemyCtrl.anime.SetTrigger ("Attack");
		//	enemyCtrl.anime.SetTrigger ("Attack");
				turn = TURN.WAIT;
				SetAIState (ENEMYSTS.WAIT, throwInterval);
			break;
			}

		}
	void OnDestroy() {

	}

	public void ThrowBall() {

		if (!enemyCtrl.activeSts) {
			return;
		}
	
		Transform ballThrew = Instantiate (ball.ball, transform.Find ("ThrowPoint").position, transform.rotation);
		ballThrew.GetComponent<BallEnemyThrow> ().speed = enemyCtrl.dir * ball.speed;
		ballThrew.GetComponent<BallEnemyThrow> ().type = ball.type;
	}
	}