using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDisplay : MonoBehaviour
{

    [SerializeField] int digitNum = 3;
    [SerializeField] GameObject elementPrefab;
    [SerializeField] Sprite[] sprites;
    [SerializeField] int changeAmount = 3;
    List<Image> images = new List<Image>();
    int destNum = 0;
    int currentNum = 0;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < digitNum; i++)
        {
            GameObject newObj = Instantiate(elementPrefab, transform);
            images.Add(newObj.GetComponent<Image>());
        }

        StartCoroutine(ChangeDisplay());
    }

    IEnumerator ChangeDisplay()
    {
        while (true)
        {
            if(currentNum == destNum)
            {

            } else
            if (currentNum < destNum - changeAmount)
            {
                currentNum += changeAmount;
            }
            else
            if (currentNum > destNum + changeAmount)
            {
                currentNum -= changeAmount;
            }
            else
            {
                currentNum = destNum;
            }

            List<int> digits = new List<int>();
            for (int i = 0; i < digitNum; i++)
            {
                int n = currentNum / (int)Mathf.Pow(10, i);
                int n2 = n % 10;
                if (n2 < 10)
                {
                    digits.Add(n2);
                }
            }

            for (int i = digitNum - 1; i >= 0; i--)
            {
                images[i].sprite = sprites[digits[digitNum - 1 - i]];
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetNumber(int num)
    {
        destNum = num;

    }
}
