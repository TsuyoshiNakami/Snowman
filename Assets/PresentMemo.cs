using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentMemo : MonoBehaviour {

    [SerializeField]SpriteRenderer presentContent;

    public void SetPresentContent(Sprite sprite)
    {
        presentContent.sprite = sprite;
    }

}
