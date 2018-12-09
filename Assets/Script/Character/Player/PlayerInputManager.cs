using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if Engineer
using Rewired;
#endif

public class PlayerInputManager : MonoBehaviour
{

    PlayerController plyCtrl;
#if Engineer
    private Player player;
#endif

    // Use this for initialization
    void Awake()
    {
#if Engineer
        player = ReInput.players.GetPlayer(0);
        Debug.Log("Rewired found " + ReInput.controllers.joystickCount + " joysticks attached.");
        for (int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            Joystick j = ReInput.controllers.Joysticks[i];
            Debug.Log(
                "[" + i + "] Joystick: " + j.name + "\n" +
                "Hardware Name: " + j.hardwareName + "\n" +
                "Is Recognized: " + (j.hardwareTypeGuid != System.Guid.Empty ? "Yes" : "No") + "\n" +
                "Is Assigned: " + (ReInput.controllers.IsControllerAssigned(j.type, j) ? "Yes" : "No")
            );
        }
#endif
        plyCtrl = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

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
        if (!plyCtrl.activeSts)
        {
            return;
        }
        Debug.Log("Enable Move");
        //上下入力で話しかける
        if (Input.GetButtonDown("Vertical") && false)
        {
            //	Debug.Log ("Vertical");

            Collider2D[] colliderList = Physics2D.OverlapPointAll(plyCtrl.targetTalkTo.position);
            foreach (Collider2D col in colliderList)
            {
                if (col.gameObject.tag == "OtherCharacter" && plyCtrl.grounded)
                {
                    Debug.Log("Talk to : " + col.gameObject.name);
                }
                break;
            }
        }
#if Engineer
        if (player.GetButtonDown("Fire"))
        {
            plyCtrl.PreThrow();
        }
        else if (player.GetButtonUp("Fire"))
        {
            plyCtrl.Throw();
        }
        float joyMv = player.GetAxis("Horizontal");//Input.GetAxis ("Horizontal");
        if (joyMv > 0)
        {
            joyMv = 1;
        }
        else if (joyMv < 0)
        {
            joyMv = -1;
        }
        plyCtrl.Move(joyMv);

        if (player.GetButtonDown("Jump"))
        {
            plyCtrl.JumpButtonDown(player.GetButtonDown("Fire"));
        }
        else if (player.GetButton("Jump"))
        {
            plyCtrl.JumpButton();
        }
        else if (player.GetButtonUp("Jump"))
        {
            plyCtrl.EndJumpUp();
        }
        if (Input.GetButtonDown("RB"))
        {
            plyCtrl.pushedRightB = true;
        }
        else if (Input.GetButtonUp("RB"))
        {
            plyCtrl.pushedRightB = false;
        }
#else


        if (Input.GetButtonDown(KeyConfig.Fire1))
        {
            plyCtrl.PreThrow();
        }
        else if (Input.GetButtonUp(KeyConfig.Fire1))
        {
            plyCtrl.Throw();
        }
        float joyMv = Input.GetAxis("Horizontal");//Input.GetAxis ("Horizontal");
        if (joyMv > 0)
        {
            joyMv = 1;
        }
        else if (joyMv < 0)
        {
            joyMv = -1;
        }
        plyCtrl.Move(joyMv);

        if (Input.GetButtonDown(KeyConfig.Jump))
        {
            plyCtrl.JumpButtonDown(Input.GetButtonDown(KeyConfig.Fire1));
        }
        else if (Input.GetButton(KeyConfig.Jump))
        {
            plyCtrl.JumpButton();
        }
        else if (Input.GetButtonUp(KeyConfig.Jump))
        {
            plyCtrl.EndJumpUp();
        }
        if (Input.GetButtonDown("RB"))
        {
            plyCtrl.pushedRightB = true;
        }
        else if (Input.GetButtonUp("RB"))
        {
            plyCtrl.pushedRightB = false;
        }
#endif
    }
}