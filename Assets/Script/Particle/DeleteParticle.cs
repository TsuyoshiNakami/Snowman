using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem particleSystem = GetComponent <ParticleSystem>();
		Destroy(this.gameObject, particleSystem.duration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
