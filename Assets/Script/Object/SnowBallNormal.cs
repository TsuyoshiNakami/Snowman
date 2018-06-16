using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallNormal : MonoBehaviour {

	Rigidbody2D rigid;
	Vector2 ThrownPos;
	public float time = 0.5f;
	Vector2 velo;
	Vector2 dis;
	Collider2D[] col;
	GameObject player;
	public float MaxThrow;
	public float lifeTime = 0.5f;
	public  float currentTime = 0;
	bool grounded = false;
	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");
        col = player.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in col)
        {
            Physics2D.IgnoreCollision(c, transform.GetComponent<Collider2D>());
        }
        StartCoroutine(CollisionControl());
    }

    IEnumerator CollisionControl()
    {

        yield return new WaitForSeconds(0.2f);

        col = player.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D c in col)
        {
            Physics2D.IgnoreCollision(c, transform.GetComponent<Collider2D>(), false);
        }

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Road")
        {
            grounded = true;
        }
    }
    //public void SetMovement(Vector2 destPos, Vector2 throwDirection, float power, float dir) {
    //       float vx = Input.GetAxis("Horizontal");
    //       float vy = Input.GetAxis("Vertical");
    //       Vector2 vec = new Vector2(vx, vy);
    //       vec = throwDirection;
    //       vec.Normalize();
    //       rigid = GetComponent<Rigidbody2D>();
    //       if(vec == Vector2.zero)
    //       {
    //           vec = new Vector2(1 * dir, 0.2f);
    //       }
    //       //rigid.velocity = vec * power;
    //       vec = throwDirection;
    //       Vector2 force = vec * rigid.mass;

    //       rigid.AddForce(force, ForceMode2D.Impulse);
    //   }


    // Update is called once per frame
    void Update () {
		if (grounded) {
			currentTime += Time.deltaTime;
		}
		if (currentTime >= lifeTime) {
			Destroy (this.gameObject);
		}
	}
}
