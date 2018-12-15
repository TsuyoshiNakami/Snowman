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

    Coroutine changeDisplayCoroutine;
    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < digitNum; i++)
        {
            GameObject newObj = Instantiate(elementPrefab, transform);
            Image image = newObj.GetComponent<Image>();
            image.sprite = sprites[0];
            images.Add(image);
        }

    }

    IEnumerator ChangeDisplay()
    {
        while (true)
        {
            if (currentNum == destNum)
            {
                Debug.Log(currentNum);
                break;
            }
            else
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

            SetNumberImages(currentNum);

            yield return new WaitForSeconds(0.01f);
        }

        if (changeDisplayCoroutine != null)
        {
            StopCoroutine(changeDisplayCoroutine);
            changeDisplayCoroutine = null;
        }
    }

    void SetNumberImages(int num)
    {
        List<int> digits = new List<int>();
        for (int i = 0; i < digitNum; i++)
        {
            int n = num / (int)Mathf.Pow(10, i);
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
    }

    public void SetNumberImmediately(int num)
    {
        if (changeDisplayCoroutine != null)
        {
            StopCoroutine(changeDisplayCoroutine);
            changeDisplayCoroutine = null;
        }
        SetNumberImages(num);
    }

    public void SetNumber(int num)
    {
        destNum = num;
        changeDisplayCoroutine = StartCoroutine(ChangeDisplay());
    }
}
