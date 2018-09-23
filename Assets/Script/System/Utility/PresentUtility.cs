using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Text;


public static class PresentUtility {
    static Dictionary<Yaku, List<string>> presentNames = new Dictionary<Yaku, List<string>>();

    public static Present GetPresentByName(string name)
    {
        return Resources.Load<Present>("Prefabs/Object/Present/" + name);
    }

    public static Present[] GetAllPresents()
    {
        return Resources.LoadAll<Present>("Prefabs/Object/Present/");

    }
    public static List<Yaku> GetAllYaku()
    {
        List<Yaku> yakus = new List<Yaku>();
        StreamReader streamReader = new StreamReader("Assets\\Resources\\Data\\YakuList.json",
        Encoding.GetEncoding("UTF-8"));
        yakus = JsonToYaku(streamReader.ReadToEnd());

        foreach(Present present in GetAllPresents())
        {
            if(present.completeYaku.yakuName.ToString() == "")
            {
                continue;
            }
            for (int i = 0; i < 3; i++) {

                present.completeYaku.presents.Add(present);
            }
            yakus.Add(present.completeYaku);
        }

        return yakus;
    }

    static List<Yaku> JsonToYaku(string jsonStr)
    {

        var json = JSON.Parse(jsonStr);
        List<Yaku> entities = new List<Yaku>();

        for (int i = 0; i < json.Count; i++)
        {
            
            var tempEntity = json[i];
            if (tempEntity["yakuName"].ToString() == "")
            {
                continue;
            }

            Yaku entity = new Yaku()
            {
                yakuName = tempEntity["yakuName"],
                score = int.Parse(tempEntity["score"]),

            };
            entity.presents = new List<Present>();
            List<string> presentNameList = new List<string>();

            for (int m = 0; m < 3; m++)
            {
                string presentName = tempEntity["presentNames"][m];
                presentNameList.Add(presentName);
            }

            presentNames.Add(entity, presentNameList);

            for (int n = 0; n < 3; n++)
            {
                entity.presents.Add(Resources.Load<Present>("Prefabs/Object/Present/" + presentNames[entity][n]));

            }
            entities.Add(entity);

        }
        return entities;
    }
    

    public static Yaku DistinguishYaku(List<Present> presents, YakuList yakuList)
    {

        Yaku maxYaku = null;
        foreach (Yaku yaku in yakuList.yakus)
        {

            List<Present> tmpPresents = new List<Present>(presents);

            //Debug.Log("役：" + yaku.yakuName);
            for (int i = 0; i < tmpPresents.Count; i++)
            {
                if (tmpPresents[0].presentName != tmpPresents[i].presentName)
                {
                    break;
                }

                if (i == tmpPresents.Count - 1)
                {
                    maxYaku = presents[0].completeYaku;
                }
            }
            foreach (uint yakuInt in yaku.GetPresentAttributeInts())
            {
                bool foundFlag = false;
                Present foundPresent = null;
                foreach (Present present in tmpPresents)
                {
                    // presentの中に aフラグが含まれているか？
                    if (present.MeetConditions(yakuInt))
                    {
                        foundFlag = true;
                        foundPresent = present;
                        break;
                    }
                    if (foundFlag)
                    {
                        break;
                    }
                }

                if (foundPresent != null)
                {

                    tmpPresents.Remove(foundPresent);
                }
                if (tmpPresents.Count == 0)
                {
                    if (maxYaku == null || yaku.score > maxYaku.score)
                    {
                        maxYaku = yaku;
                    }
                    break;
                }
            }
        }
        return maxYaku;
    }


}
