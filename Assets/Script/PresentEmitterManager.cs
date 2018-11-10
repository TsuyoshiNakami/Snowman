using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PresentEmitterManager : MonoBehaviour {

    [SerializeField] bool generatePresent = true;
    [SerializeField]float generateInterval = 3;




    [SerializeField] Vector2 emitRangeMin;
    [SerializeField] Vector2 emitRangeMax;

    [Inject]
    PresentManager presentManager;

    [Inject]
    PresentGameManager presentGameManager;

    [Inject]
    IPresentGameDirector gameDirector;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if(!presentGameManager.enablePresentEmit)
        {
            return;
        }

        gameDirector.PresentEmitUpdate();
	}
}
