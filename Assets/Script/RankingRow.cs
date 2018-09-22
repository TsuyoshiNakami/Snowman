using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingRow : MonoBehaviour {

    TextMeshProUGUI rankText;
    TextMeshProUGUI nameText;
    TextMeshProUGUI scoreText;

    public void Initialize()
    {
        rankText = transform.Find("RankText").GetComponent<TextMeshProUGUI>();
        nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    public void SetText(int rank, string nameText, int score)
    {
        rankText.text = rank + 1 + "位";
        this.nameText.text = nameText;
        scoreText.text = score + "";
    }
}
