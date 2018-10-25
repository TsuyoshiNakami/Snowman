using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEaterEmitter : MonoBehaviour {

    [SerializeField] bool emitFoodEater = false;
    [SerializeField] Transform presentEaterPosition;
    [SerializeField] public float emitStartTime = 10;

    public bool isStartedEmit = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(emitFoodEater && isStartedEmit && Random.Range(0, 1000) < 1)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/Mouse"), presentEaterPosition.position, Quaternion.identity);
        }
	}
}
