using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetEmitterType
{
    RandomInRectangle
}
public class EnemyTargetEmitter : MonoBehaviour {
    [SerializeField] float interval = 1;
    [SerializeField] GameObject enemy;
    [SerializeField] TargetEmitterType type = TargetEmitterType.RandomInRectangle;
    [SerializeField] Vector2 range;
    [SerializeField] bool emitButton = true;
    float timer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        switch (type)
        {
            case TargetEmitterType.RandomInRectangle:
                
                Vector2 newPos = new Vector2(Random.Range(-range.x, range.x), Random.Range(-range.y, range.y));
                if (!emitButton)
                {
                    if (timer >= interval)
                    {
                        Instantiate(enemy, (Vector2)transform.position + newPos, transform.rotation);
                        timer = 0;
                    }
                } else
                {
                    if(Input.GetButtonDown("Joy1"))
                    {
                        Instantiate(enemy, (Vector2)transform.position + newPos, transform.rotation);

                    }
                }
                break;
        }

        //if (timer >= interval)
        //{
        //    Instantiate(enemy, transform.position, transform.rotation);
        //    timer = 0;
        //}
	}
}
