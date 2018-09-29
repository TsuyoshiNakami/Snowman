using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkObj : MonoBehaviour {

    Rigidbody2D rigidbody2D;
    float timer = 3;
    float dir = 1;
    [SerializeField] float speed = 10;

    bool grounded = false;
	// Use this for initialization
	void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (grounded)
        {
            rigidbody2D.velocity = new Vector2(dir, 0) * speed;
            transform.rotation = Quaternion.identity;
        }

        if (timer <= 0)
        {

            timer = Random.Range(1, 4);
            dir = Random.Range(0, 2) == 0 ? 1 : -1;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Road")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Road")
        {
            grounded = false;
        }
    }
}
