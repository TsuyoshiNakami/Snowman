using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bakeable : MonoBehaviour {

    public float minBakeTime = 1;
    public float maxBakeTime = 3;
    [SerializeField] GameObject bakedObj;
    [SerializeField] GameObject burntObj;
    public void Bake(float bakeTime)
    {
        if(bakeTime < minBakeTime)
        {
            //生焼け

        } else if(bakeTime <= maxBakeTime)
        {
            //完成\
            if (bakedObj)
            {
                Instantiate(bakedObj, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        } else
        {
            //焦げた
            if (burntObj)
            {
                Instantiate(burntObj, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(.3f, .3f, .0f);
            }
        }
    }
}
