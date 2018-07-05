using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Throwable : MonoBehaviour
{
    [SerializeField] bool IgnorePlayer = false;
    [SerializeField] float gravity = 4;
    Rigidbody2D rigid;
    Collider2D collider2D;
    GameObject playerObj;
    GameObject holdObj = null;

    bool isTaken = false;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        collider2D = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = gravity;

        if (IgnorePlayer)
        {
        }
    }

    public void IgnoreCollision(GameObject obj, bool flag)
    {
        foreach (Collider2D c in obj.GetComponentsInChildren<Collider2D>())
        {
            Debug.Log(c.name);
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), c, flag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (holdObj != null)
        {
            transform.position = holdObj.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(!Input.GetButton(KeyConfig.Fire1))
        {
            return;
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {

    }

    IEnumerator CollisionControl()
    {
        yield return new WaitForSeconds(0.2f);
    }

    public void OnHeld(GameObject holdObj)
    {
        this.holdObj = holdObj;
    }
    public void OnThrew(Vector2 destPos, Vector2 throwDirection, float power, float dir)
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

        vec = throwDirection;
        Vector2 force = vec * rigid.mass;
        //StartCoroutine(CollisionControl());
        holdObj = null;
        rigid.velocity = vec;
        //rigid.AddForce(force, ForceMode2D.Impulse);
    }
}
