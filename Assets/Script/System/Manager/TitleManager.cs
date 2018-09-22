using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    RankingManager rankingManager;

	// Use this for initialization
	void Start () {
        rankingManager = GameObject.Find("RankingManager").GetComponent<RankingManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.S))
        {
            rankingManager.ShowRanking();
        }
	}
}
