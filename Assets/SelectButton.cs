using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectButton : MonoBehaviour
{

    Image image;
    Button button;
    [SerializeField] Sprite spriteNotSelected;
    [SerializeField] Sprite spriteSelected;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = spriteNotSelected;
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            image.sprite = spriteSelected;
        }
        else
        {
            image.sprite = spriteNotSelected;
        }
    }
}
