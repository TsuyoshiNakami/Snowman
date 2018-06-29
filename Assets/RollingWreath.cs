using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RollingDirection
{
    Left,
    Right
}
public class RollingWreath : MonoBehaviour {

    [SerializeField]float speed = 2;
    [SerializeField]  RollingDirection rollingDirection = RollingDirection.Left;
    float actualSpeed;

    List<Transform> ridingCharacters;
	// Use this for initialization
	void Start () {
        ridingCharacters = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		switch(rollingDirection)
        {
            case RollingDirection.Left:
                actualSpeed = speed;
                break;
            case RollingDirection.Right:
                actualSpeed = -speed;
                break;
        }
        transform.Rotate(0, 0, actualSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        ridingCharacters.Add(c.transform);
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        c.transform.RotateAround(transform.position, Vector3.forward, actualSpeed * Time.deltaTime);
        Vector2 verticalVector = c.transform.position - transform.position;
        float degree = Vector2.Angle(Vector2.up, verticalVector);
        if (verticalVector.x > 0) degree = -degree;
        c.transform.rotation = Quaternion.Euler(0, 0, degree);

    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.transform != null)
        {
            ridingCharacters.Remove(c.transform);
            StartCoroutine(InitializeRotation(c.transform));
        }
    }

    IEnumerator InitializeRotation(Transform trans)
    {
        if (trans == null)
        {
            ridingCharacters.Remove(trans);
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
            if (!ridingCharacters.Contains(trans) && trans != null)
            {
                trans.rotation = new Quaternion(0, 0, 0, 1);
            }
            //ゆっくり元の角度に戻る
            //float startAngle = trans.rotation.eulerAngles.z;
            //for (int i = 0; i < 5; i++)
            //{
            //    yield return new WaitForSeconds(0.01f);
            //    if (!ridingCharacters.Contains(trans))
            //    {
            //        //    trans.rotation = Quaternion.Euler(Vector3.forward  * startAngle * -1 / 5);
            //    }
            //}
        }
    }
}
