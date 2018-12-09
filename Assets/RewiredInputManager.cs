using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewiredInputManager : MonoBehaviour
{
    [SerializeField] GameObject eventSystemDefault;
    [SerializeField] GameObject eventSystemRewired;

    static GameObject rewiredInputManager;
    // Use this for initialization
    void Start()
    {
#if Engineer
        if(eventSystemRewired != null) {
        eventSystemRewired.SetActive(true);
        }
        if (rewiredInputManager == null)
        {
        //    rewiredInputManager = gameObject;
        }else
        {
            Destroy(gameObject);
        }
#else
        if (eventSystemDefault != null)
        {
            eventSystemDefault.SetActive(true);
        }
        Destroy(gameObject);
#endif
    }
}
