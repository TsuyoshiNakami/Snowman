using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeElement : MonoBehaviour {

    Sprite[] presentSprites = new Sprite[3];
    string yakuName = "";
    int score = 0;
    [SerializeField] TextMeshProUGUI yakuNameText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] images;

	// Use this for initialization
	void Start () {
        //yakuNameText = transform.Find("Texts/NameText").GetComponent<TextMeshProUGUI>();
        //scoreText = transform.Find("Texts/ScoreText").GetComponent<TextMeshProUGUI>();
        //for(int i = 0; i < 3; i++)
        //{
        //   // images[i] = transform.Find("Images/Image" + i).GetComponent<Image>();
        //}
	}
	

	// Update is called once per frame
	void Update () {
		
	}

    public void SetUI(Yaku yaku)
    {
        if (ES3.KeyExists(yaku.yakuName, PresentGameConsts.saveSetting))
        {
            yakuNameText.text = yaku.yakuName;
            scoreText.text = yaku.score + "";
        } else
        {
            yakuNameText.text = "???";
            scoreText.text = "???";
        }

            for (int i = 0; i < 3; i++)
            {
                images[i].sprite = yaku.presents[i].GetComponent<SpriteRenderer>().sprite;
            }
        
    }

}
