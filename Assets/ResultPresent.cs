using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ResultPresent : MonoBehaviour {

    Rigidbody2D rigid;
    [SerializeField] Vector2 minForce;
    [SerializeField] Vector2 maxForce;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody2D>();
        float x = Random.Range(minForce.x, maxForce.x);
        float y = Random.Range(minForce.y, maxForce.y);
        rigid.velocity = new Vector2(x, y);
        Invoke("Destroy", 2);
	}

    [SerializeField] float speedRight;
    private void Update()
    {
        rigid.velocity += new Vector2(speedRight * Time.deltaTime, 0);
    }
    void Destroy()
    {
        Destroy(gameObject);
    }

}
