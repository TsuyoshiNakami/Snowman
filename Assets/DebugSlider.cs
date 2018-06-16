using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugSlider : MonoBehaviour {
    PlayerController playerController;
    Slider slider;
    [SerializeField] float max = 24;
    [SerializeField] float defaultValue = 24;
    [SerializeField] DebugTextType type = DebugTextType.ThrowPower;
    [SerializeField] SnowBallNormal snowBall;
    // Use this for initialization
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        slider = GetComponent<Slider>();
        slider.maxValue = max;

        switch (type)
        {
            case DebugTextType.ThrowPower:
                slider.value = playerController.maxThrowPower / max;
                break;
            case DebugTextType.ChargeTime:
                slider.value = playerController.chargeTime;
                break;
            case DebugTextType.SnowBallGravity:
                slider.value = snowBall.GetComponent<Rigidbody2D>().gravityScale;
                break;
        }
        slider.value = defaultValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 ||
            Input.GetAxis("Vertical") != 0)
        {
            slider.interactable = false;
        }
        else
        {
            slider.interactable = true;
        }
        switch (type)
        {
            case DebugTextType.ThrowPower:
                playerController.maxThrowPower = slider.value;
                break;
            case DebugTextType.ChargeTime:
                playerController.chargeTime = slider.value;
                break;
            case DebugTextType.SnowBallGravity:
                snowBall.GetComponent<Rigidbody2D>().gravityScale = slider.value;

                break;
        }

    }
}
