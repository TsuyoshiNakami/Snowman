using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroller : MonoBehaviour {

    [SerializeField]List<ResultBackGround> backs = new List<ResultBackGround>();
    [SerializeField] float borderX;
    [SerializeField] float scrollSpeed = 3;
    ResultBackGround currentTop;
	// Use this for initialization
	void Start () {
        for(int i = 1; i < backs.Count; i++)
        {
            backs[i].index = i;
            backs[i].forward = backs[i - 1].transform;
        }
        currentTop = backs[0];
        backs[0].index = 0;
        backs[0].forward = backs[backs.Count - 1].transform;
	}
	
	// Update is called once per frame
	void Update () {
        currentTop.transform.Translate(Vector2.right * scrollSpeed * Time.deltaTime);

        for(int i = currentTop.index; i < backs.Count + currentTop.index; i++)
        {
            int num = i;
            if(i + currentTop.index >= backs.Count)
            {
                num %= backs.Count;
            }
            if(num == currentTop.index)
            {
                continue;
            }
            backs[num].transform.position = backs[num].forward.position - Vector3.right * GetComponentInChildren<SpriteRenderer>().bounds.size.x;
        }
        if(currentTop.transform.position.x > borderX)
        {
            int behindNum = currentTop.index - 1;
            if(behindNum < 0)
            {
                behindNum = backs.Count - 1;
            }
            currentTop.transform.position = backs[behindNum].transform.position - Vector3.right * GetComponentInChildren<SpriteRenderer>().bounds.size.x;
            int forwardNum = currentTop.index + 1;
            if(forwardNum >= backs.Count)
            {
                forwardNum = 0;
            }
            currentTop = backs[forwardNum];
        }
	}
}
