using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTargetType
{
    Pause,
    Move
}

interface ThrowTarget
{

}
public class EnemyTarget : MonoBehaviour, ThrowTarget
{
    public bool isDead = false;
    [SerializeField] float speed =  2;
    public EnemyTargetType type = EnemyTargetType.Pause;
    float vy;
    float timer = 0;
    private Rigidbody2D rigid;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        switch (type)
        {
            case EnemyTargetType.Pause:
                speed = 0;
                break;
        }
        transform.Translate(new Vector2(speed, vy) * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.transform.tag == "PlayerArmBullet")
        {
            Vector2 vec = transform.position - c.transform.position;
            vec.Normalize();
            NockBack(vec);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySEOneShot("PlayerBulletHit");
            Destroy(c.gameObject);
            Destroy(gameObject, 0.5f);
            isDead = true;
        }
    }

    void NockBack(Vector2 power)
    {
        rigid.AddForce(power * 50);
    }
}
