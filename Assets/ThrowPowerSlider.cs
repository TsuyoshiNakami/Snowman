using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowPowerSlider : MonoBehaviour {
    PlayerController playerController;
    Slider slider;
    [SerializeField] float maxThrowPower = 24;
	// Use this for initialization
	void Start () {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        slider = GetComponent<Slider>();
        slider.value = playerController.maxThrowPower / maxThrowPower;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0 ||
            Input.GetAxis("Vertical") != 0)
        {
            slider.interactable = false;
        }
        else
        {
            slider.interactable = true;
        }

        playerController.maxThrowPower = slider.value * maxThrowPower;
	}
}
