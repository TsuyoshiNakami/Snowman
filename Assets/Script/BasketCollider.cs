using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class BasketCollider : MonoBehaviour
{

    Collider2D collider;
    Collider2D entranceCollider;
    Collider2D[] nowCollider = new Collider2D[6];
    Collider2D[] oldCollider = new Collider2D[6];

    Subject<GameObject> ItemEnterSubject = new Subject<GameObject>();
    public IObservable<GameObject> OnItemEnter
    {
        get { return ItemEnterSubject; }
    }

    bool isInCollider = false;

    int ballCount = 0;
    [SerializeField] int score = 1;
    // Use this for initialization
    void Awake()
    {

        collider = GetComponent<BoxCollider2D>();
        entranceCollider = transform.Find("Entrance").GetComponentInChildren<BoxCollider2D>();

    }


    public void CheckCollision()
    {
        GetCollider();

        Collider2D[] exitCollider = new Collider2D[6];
        int count = 0;
        foreach (Collider2D c in oldCollider)
        {
            if (!nowCollider.Contains(c))
            {
                exitCollider[count] = c;
                count++;
            }
        }

        ContactFilter2D filter;
        Collider2D[] colliders = new Collider2D[6];
        filter = new ContactFilter2D();
        collider.OverlapCollider(filter, colliders);

        count = 0;
        foreach (Collider2D c in colliders)
        {
            if (c == null) continue;
            if (exitCollider.Contains(c))
            {
                ItemEnter(c.gameObject);
            }
        }

        oldCollider = nowCollider;
        nowCollider = new Collider2D[6];
    }

    void ItemEnter(GameObject item)
    {
        ItemEnterSubject.OnNext(item);

        ballCount++;
        //GameManager.score += score;
        //UIManager.SetText("BallCountText", "入った数：" + ballCount);
    }

    void GetCollider()
    {
        if (entranceCollider == null)
        {
            entranceCollider = transform.Find("Entrance").GetComponentInChildren<BoxCollider2D>();
        }
        ContactFilter2D filter;
        Collider2D[] results = new Collider2D[6];
        filter = new ContactFilter2D();
        entranceCollider.OverlapCollider(filter, results);

        int count = 0;
        foreach (Collider2D c in results)
        {
            if (c == null) continue;

            if (c.tag == "Throwable" && c.gameObject.GetComponent<Throwable>().hasBeThrew && !c.gameObject.GetComponent<Throwable>().IsTaken)
            {
                nowCollider[count] = c;
                count++;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D c)
    {

    }
}
