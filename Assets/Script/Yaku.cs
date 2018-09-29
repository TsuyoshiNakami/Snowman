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
    SoccerBall = 1 << 5,
    Toy = 1 << 6,
    Cat = 1 << 7,
    Sake = 1 << 8,
    BaseBall = 1 << 9,
    Sports = 1 << 10,
    Animal = 1 << 11,
    Food = 1 << 12,
    PikopikoHummer = 1 << 13,
    Robot = 1 << 14,
    Kendama = 1 << 15,
    Hiyoko = 1 << 16,
    GingerBread = 1 << 17,
    Monaka = 1 << 18
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
    [SerializeField] public List<Present> presents = new List<Present>();
    public List<string> PresentNames {
        get {
            List<string> tmp = new List<string>();
            foreach(Present present in presents)
            {
                tmp.Add(present.presentName);
            }
            return tmp;
        }
    }
    [NonSerialized] uint[] presentAttributeInt = new uint[3];
    public int score;
    [SerializeField] public List<string> presentNames = new List<string>();
    
    public uint[] GetPresentAttributeInts()
    {
        for(int i = 0; i < presentAttributeInt.Length; i++)
        {
            presentAttributeInt[i] = presents[i].AttributesToUInt();
        }
        return presentAttributeInt;
    }
}
