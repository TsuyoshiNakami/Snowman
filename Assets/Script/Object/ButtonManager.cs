using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {
	public ButtonEvent[] buttons;
	bool allClear = false;
	[SerializeField] appearObjectInfo[] appearObjectList;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (allClear)
			return;


		for (int i = 0; i < buttons.Length; i++) {
			if (!buttons [i].Switched) {
				return;
			}
		}

		allClear = true;
		Invoke ("wineClear", 1f);
		Debug.Log ("さいこう！");
	}

	void wineClear() {
		if (appearObjectList.Length > 0) {
			foreach (appearObjectInfo appearObject in appearObjectList) {
				appearObject.appearObj.SetActive (true);
				//Instantiate (appearObject.appearObj, appearObject.position, transform.rotation);
			}
		}
	}
}
