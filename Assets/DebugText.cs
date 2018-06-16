using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DebugTextType
{
    ThrowPower,
    ChargeTime,
    SnowBallGravity
}

public class DebugText : MonoBehaviour {
    Text text;
    PlayerController playerController;
    [SerializeField] SnowBallNormal snowBall;
    [SerializeField] DebugTextType type = DebugTextType.ThrowPower;
    // Use this for initialization
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type) {
            case DebugTextType.ThrowPower:
        text.text = string.Format("{0:f2}", playerController.throwPower) + "/" + playerController.maxThrowPower;
                break;
            case DebugTextType.ChargeTime:

                text.text = string.Format("{0:f2}", playerController.chargeTime);
                break;
            case DebugTextType.SnowBallGravity:
                text.text = string.Format("{0:f2}", snowBall.GetComponent<Rigidbody2D>().gravityScale);
                break;
        }
    }
}
