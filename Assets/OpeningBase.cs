using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(CheckOpeningReload());

    }
	
    IEnumerator CheckOpeningReload()
    {
        GameObject openingManagerObj = null;
        while (openingManagerObj == null)
        {
            openingManagerObj = GameObject.Find("OpeningManager");
            yield return new WaitForSeconds(1);
        }
        openingManagerObj.GetComponent<OpeningManager>().StartOpening();
    }
}
