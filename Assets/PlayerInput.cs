using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInput : MonoBehaviour {
    public GameObject snowBallThrown;   //投げた雪玉
    public GameObject target;
    public float speed = 3;
    float time = 0;
	// Update is called once per frame
    void Start()
    {
        target.SetActive(false);
    }

	void Update () {
        Transform point = transform;

        if (Input.GetButtonDown("Fire1"))
        {
            target.SetActive(true);
            target.transform.position = (Vector2)transform.position + Vector2.right * 4f;
            time = 6 / speed * 3 / 4;
        }else if(Input.GetButton("Fire1"))
        {
            Debug.Log(time);
            Vector2 pos = transform.position;

            float radius = 4;
            time += Time.deltaTime;
            pos.x -= Mathf.Sin(time * speed) * radius;
            pos.y += Mathf.Cos(time * speed) * radius;
            target.transform.position = (pos);
        }
        if(Input.GetButtonUp("Fire1"))
        {
            GameObject sbThrown = Instantiate(snowBallThrown, point.position, point.rotation);

            sbThrown.GetComponent<SnowBallThrown>().SetMovement(target.transform.position);
        }
	}

}
