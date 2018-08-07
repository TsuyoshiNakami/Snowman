using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum YakuEnum
{
    Sweets,

}

[Flags]
public enum PresentAttribute {
    None = 0 << 0,
    Parfait = 1 << 0,
    Cake = 1 << 1,
    Donut = 1 << 2,
    Brandy = 1 << 3,
    Sweet = 1 << 4,
}
public static class PresentAttributeExtensions
{
    public static bool MeetConditions(this PresentAttribute a, PresentAttribute b)
    {
        return ((a & b) == b);
    }

    public static bool MeetConditions(this PresentAttribute a, uint b)
    {
        uint aUint = (uint)(int)a;
        return ((aUint & b) == b);
    }
}

[System.Serializable]
public class Yaku {
    [SerializeField]
    public string yakuName = "";
    public YakuEnum yakuEnum;
    public Present[] presents;
    [NonSerialized] uint[] presentAttributeInt = new uint[3];
    public int score;

    
    public uint[] GetPresentAttributeInts()
    {
        for(int i = 0; i < presentAttributeInt.Length; i++)
        {
            presentAttributeInt[i] = presents[i].AttributesToUInt();
        }
        return presentAttributeInt;
    }
}
