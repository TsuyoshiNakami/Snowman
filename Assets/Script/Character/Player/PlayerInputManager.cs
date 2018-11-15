using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour {

    PlayerController plyCtrl;
	// Use this for initialization
	void Awake () {
		plyCtrl = GetComponent<PlayerController> ();

	}

	// Update is called once per frame
	void Update () {
		if (!plyCtrl.isStarted)
			return;

        if (Pauser.isPausing)
        {
            plyCtrl.Move(0);
            plyCtrl.EndJumpUp();
            plyCtrl.ThrowCancel();
            return;
        }
		//操作可能か
		if (!plyCtrl.activeSts) {
			return;
		}
		//上下入力で話しかける
		if(Input.GetButtonDown ("Vertical") && false) {
		//	Debug.Log ("Vertical");

			Collider2D[] colliderList = Physics2D.OverlapPointAll (plyCtrl.targetTalkTo.position);
			foreach (Collider2D col in colliderList) {
				if (col.gameObject.tag == "OtherCharacter" && plyCtrl.grounded) {
					Debug.Log ("Talk to : " + col.gameObject.name);
				}
				break;
			}
		}
		if (Input.GetButtonDown (KeyConfig.Fire1)) {
			plyCtrl.PreThrow ();
		} else if (Input.GetButtonUp(KeyConfig.Fire1))
        {
            plyCtrl.Throw();
        }
		float joyMv = Input.GetAxis ("Horizontal");
        if(joyMv > 0)
        {
            joyMv = 1;
        } else if (joyMv < 0)
        {
            joyMv = -1;
        }
		plyCtrl.Move (joyMv);

		if (Input.GetButtonDown (KeyConfig.Jump)) {
			plyCtrl.JumpButtonDown ();
		} else if(Input.GetButton (KeyConfig.Jump)) {
			plyCtrl.JumpButton ();
		}else if(Input.GetButtonUp (KeyConfig.Jump)){
			plyCtrl.EndJumpUp ();
		}
        if(Input.GetButtonDown("RB"))
        {
            plyCtrl.pushedRightB = true;
        } else if (Input.GetButtonUp("RB"))
        {
            plyCtrl.pushedRightB = false;
        }
	}
}