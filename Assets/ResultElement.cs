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

    public void SetUI(YakuResult result)
    {
            yakuNameText.text = result.yaku.yakuName;
            scoreText.text = result.yaku.score + "" + " × " + result.count;
            for (int i = 0; i < 3; i++)
            {
                images[i].sprite = result.presents[i].sprite;
            }
    }

}
