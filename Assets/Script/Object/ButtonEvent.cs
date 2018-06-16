using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class appearObjectInfo {
	[SerializeField] public GameObject appearObj;
	[SerializeField] public Vector2 position;
}
public class ButtonEvent : MonoBehaviour {
	[SerializeField] GameObject[] destroyObjectList;
	[SerializeField] appearObjectInfo[] appearObjectList;

	[SerializeField] int count = 0;
	[System.NonSerialized] public bool Switched = false;

	// Use this for initialization
	void Start () {
		
	}
	

	void Hitted() {
		if (appearObjectList.Length > 0) {
			foreach (appearObjectInfo appearObject in appearObjectList) {
				appearObject.appearObj.SetActive (true);
				//Instantiate (appearObject.appearObj, appearObject.position, transform.rotation);
			}
		}
		if (destroyObjectList.Length > 0) {
			foreach (GameObject destroyObject in destroyObjectList) {
				Destroy (destroyObject, 1.0f);
			}
		}


		count++;
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (!Switched) {

			if (col.CompareTag ("PlayerArmBullet")) {
				Destroy (col.gameObject);
				Switched = true;
				GameObject.Find ("SoundManager").GetComponent<SoundManager> ().PlaySEOneShot ("Button");
			//	GetComponent<SpriteRenderer> ().color = Color.red;
				Hitted();

			}
		}
	}
}
