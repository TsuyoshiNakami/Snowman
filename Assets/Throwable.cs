using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] bool IgnorePlayer = false;
    Rigidbody2D rigid;
    GameObject player;
    PlayerController playerController;
    Collider2D collider2D;

    bool isTaken = false;
    // Use this for initialization
    void Start()
    {

    }

    void Initialize()
    {
        collider2D = GetComponent<Collider2D>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        rigid = GetComponent<Rigidbody2D>();
        if (IgnorePlayer)
        {

            foreach (Collider2D c in player.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), c);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(playerController.throwObj == gameObject)
        {
            isTaken = true;
            collider2D.isTrigger = true;
            transform.position = playerController.throwPoint.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D c)
    {
        if(!Input.GetButton(KeyConfig.Fire1))
        {
            return;
        }
        if(c.transform.tag == "PlayerBody")
        {
            playerController.SetThrowObj(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.transform.tag == "PlayerBody" && !isTaken)
        {
            playerController.SetThrowObj();
        }
    }
    IEnumerator CollisionControl()
    {

        yield return new WaitForSeconds(0.2f);

        foreach (Collider2D c in player.GetComponentsInChildren<Collider2D>())
        {
            Physics2D.IgnoreCollision(c, transform.GetComponent<Collider2D>(), false);
        }

    }

    public void SetMovement(Vector2 destPos, Vector2 throwDirection, float power, float dir)
    {
        Initialize();
        isTaken = false;
        collider2D.isTrigger = false;
        Vector2 vec = throwDirection;
        vec.Normalize();

        if (vec == Vector2.zero)
        {
            vec = new Vector2(1 * dir, 0.2f);
        }
        //rigid.velocity = vec * power;
        vec = throwDirection;
        Vector2 force = vec * rigid.mass;

        StartCoroutine(CollisionControl());

        rigid.AddForce(force, ForceMode2D.Impulse);
    }
}
