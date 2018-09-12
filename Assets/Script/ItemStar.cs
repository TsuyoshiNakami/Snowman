using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStar : Item {

	// Use this for initialization
	void Start () {
		
	}
    protected override void ActEnter()
    {
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySEOneShot("Coin");
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
