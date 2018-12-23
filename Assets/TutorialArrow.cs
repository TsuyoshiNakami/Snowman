using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour {

    RectTransform rectTransform;
    [SerializeField] Vector3 offset;
    [SerializeField] float interval = 0.3f;
	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(Shake());
	}
	
    IEnumerator Shake()
    {
        WaitForSeconds wait = new WaitForSeconds(interval);
        Vector3 init = rectTransform.localPosition;

        while(true)
        {
            rectTransform.localPosition = init;
            yield return wait;
            rectTransform.localPosition = init + offset;
            yield return wait;
        }
    }

}
