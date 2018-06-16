using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPile : MonoBehaviour {

	public GameObject[] ObjectList;
	public GameObject[] AppearItem;
	// Use this for initialization
	void Start () {
		InvokeRepeating ("CheckList", 1f, 1f);
	}
	
	void CheckList() {
		foreach(GameObject obj in ObjectList) {
			if (obj != null)
				return;
		}
		foreach(GameObject obj in AppearItem) {
			obj.SetActive (true);
		}
		GameObject.Find ("SoundManager").GetComponent<SoundManager> ().PlaySEOneShot ("Button");
		Destroy (gameObject);
	}

}
