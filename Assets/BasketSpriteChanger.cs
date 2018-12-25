using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketSpriteChanger : MonoBehaviour {
    [System.Serializable]
    public enum Color
    {
        B,
        Y,
        P,
        G
    }

    [SerializeField] string spriteFolderPath;
    Sprite[] sprites;
    SpriteRenderer spriteRenderer;
    [SerializeField] public Color color;

	// Use this for initialization
	void Start () {
        sprites = Resources.LoadAll<Sprite>(spriteFolderPath);
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    Sprite GetSprite(string name)
    {
        foreach(Sprite sprite in sprites)
        {
            if(sprite.name == name)
            {
                return sprite;
            }
        }
        Debug.Log("Sprite " + name + " not found");
        return null;
    }

	// Update is called once per frame
	void LateUpdate () {
        string name = spriteRenderer.sprite.name;
        string[] names = name.Split('_');
        names[0] = "present" + color.ToString();
        spriteRenderer.sprite = GetSprite(names[0] + "_" + names[1] + "_" + names[2]+ "_" + names[3]);
	}
}
