using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketMover : MonoBehaviour {
    BasketHinge hinge;
    Transform hingeObj;
    BasketCollider collider;

    public float closeTime = 8;
    public float openTime = 6;

    bool useHinge = false;

    float timer = 0;
    [SerializeField] float speed = 5;
    [SerializeField] GameObject[] checkPoints;
    int pointNum = 0;

    // Use this for initialization
    void Start () {

        hingeObj = transform.Find("Hinge");
        if (hingeObj != null)
        {
            useHinge = true;
            hinge = hingeObj.GetComponent<BasketHinge>();
        }
        collider = GetComponent<BasketCollider>();
    }
	
	// Update is called once per frame
	void Update () {
        Move();



        if (hinge != null)
        {
            ManageHinge();
            if (hinge.isClosed) return;
        }


        collider.CheckCollision();
	}

    void ManageHinge()
    {
        timer += Time.deltaTime;
        if (!hinge.isClosed)
        {
            if (closeTime == 0)
            {
                return;
            }
            if (timer > openTime)
            {
                hinge.BeginClose();
                timer = 0;
            }
        }
        else
        {

            if (timer > closeTime)
            {
                hinge.BeginOpen();
                timer = 0;
            }
        }
    }
    void Move()
    {
        if (checkPoints.Length <= 0) return;
        Vector3 vec = checkPoints[(pointNum + 1) % checkPoints.Length].transform.position - checkPoints[pointNum % checkPoints.Length].transform.position;

        transform.position += vec.normalized * speed * Time.deltaTime;

        float distance = (checkPoints[(pointNum + 1) % checkPoints.Length].transform.position - transform.position).magnitude;

        if (distance < speed * Time.deltaTime)
        {
            pointNum++;
        }

    }
}
