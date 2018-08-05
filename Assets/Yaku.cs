using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum YakuEnum
{
    Sweets,

}

public enum PresentAttribute {
    Parfait,
    Cake,
    Donut,
    Brandy,
    Sweet,
}


[System.Serializable]
public class Yaku {
    [SerializeField]
    public string yakuName = "";
    public YakuEnum yakuEnum;
    public PresentAttribute[] attribute;
    public int score;
}
