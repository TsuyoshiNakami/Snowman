using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PresentEater : MonoBehaviour {

    PresentManager presentManager;
    Present targetFood;
    bool closeToTarget = false;
    float eatingTime = 0;
	// Use this for initialization
	void Start () {
        presentManager = GameObject.Find("PresentManager").GetComponent<PresentManager>();
        InvokeRepeating("FindFood", 0, 1);
        GetComponent<Throwable>().OnThrewEvent.Subscribe(_ => {
            Destroy(gameObject);
        });
        Collider2D col = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(col, GameObject.Find("Player/ColliderBody").GetComponent<Collider2D>());
    }
	
    void FindFood()
    {
        List<Present> foods = presentManager.GetPresentByAttribute(PresentAttribute.Food);
        Present present = null;
            if(foods.Count > 0)
        {
            present = foods[0];
        }

        if(present != null)
        {
            targetFood = present;
        } else
        {
            Debug.Log("食べ物なし");
        }

    }

    float speed = 10;
	// Update is called once per frame
	void Update () {
	    if(targetFood != null)
        {
            // ターゲットの食べ物に近づいているか？
            closeToTarget = Vector3.Distance(targetFood.transform.position, transform.position) < 1 ? true : false;
            if(closeToTarget)
            {
                eatingTime += Time.deltaTime;
            }
            if(eatingTime > 4)
            {
                eatingTime = 0;
                presentManager.HidePresentFromView(targetFood.gameObject);
                Destroy(targetFood.gameObject);
 
            }
            if(!closeToTarget)
            {
                eatingTime = 0;
                float dir = 0;
                dir = targetFood.transform.position.x < transform.position.x ? -1 : 1;

                transform.Translate(Vector3.right * speed * dir * Time.deltaTime);
            } 
        }	
	}
}
