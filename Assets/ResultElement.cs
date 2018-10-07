using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultElement : MonoBehaviour
{

    Sprite[] presentSprites = new Sprite[3];
    string yakuName = "";
    int score = 0;
    [SerializeField] TextMeshProUGUI yakuNameText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] images;

    public void SetUI(Yaku yaku, List<Present> presents)
    {
            yakuNameText.text = yaku.yakuName;
            scoreText.text = yaku.score + "";
            for (int i = 0; i < 3; i++)
            {
                images[i].sprite = presents[i].sprite;
            }
    }

}
