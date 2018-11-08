using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GarbageBin : MonoBehaviour {
    [Inject]
    PresentManager presentManager;

    private void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if(c.gameObject.CompareTag("Throwable"))
        {
            presentManager.DeletePresent(c.gameObject);
        }
    }
}
