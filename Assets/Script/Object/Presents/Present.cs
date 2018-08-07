using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour{
    public string presentName = "";
    public PresentAttribute[] attributes;

    public uint AttributesToUInt ()
    {
        uint tmp = 0;
        foreach(PresentAttribute a in attributes)
        {
            tmp += (uint)(int)a;
        }

        return tmp;
    }

    public bool MeetConditions(uint b)
    {
        uint aUint = AttributesToUInt();
        return ((aUint & b) == b);
    }
}
