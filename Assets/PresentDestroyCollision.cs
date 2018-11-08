using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PresentDestroyCollision : MonoBehaviour {
    [Inject]
    PresentManager presentManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Present>() != null)
        {
            presentManager.DeletePresent(collision.gameObject);
        }

        if(collision.GetComponent<PresentDeliverer>() != null)
        {
            Destroy(gameObject);
        }
    }
}
