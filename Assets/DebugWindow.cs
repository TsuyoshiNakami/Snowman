using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugWindow : MonoBehaviour
{
    [SerializeField] bool isDebuging = false;
    Text text;
    // Use this for initialization
    void Start()
    {
        if (!isDebuging)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            return;
        }

        text = GetComponentInChildren<Text>();
    }

    public void SetText(string param)
    {
        text.text = param;
    }

    void Update()
    {
        if (Input.GetButtonDown("L3"))
        {
            if (!isDebuging)
            {
                foreach (Transform child in transform)
                {


                    child.gameObject.SetActive(true);
                }
            }
        
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        isDebuging = !isDebuging;
    }
    }
}
