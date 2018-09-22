using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class YakuList : MonoBehaviour {

    [SerializeField]public List<Yaku> yakus;
    public Yaku defaultYaku;

    private void Start()
    {
        //List<Yaku> saveYakus = new List<Yaku>();
        //foreach (Yaku yaku in yakus)
        //{
        //    List<string> presentNames = new List<string>();
        //    for(int i = 0; i < 3; i++)
        //    {
        //        //List<string> tmp = new List<string>();
        //        //foreach (Present present in yaku.presents)
        //        //{
        //        //    tmp.Add(present.presentName);
        //        //}
        //        presentNames = yaku.PresentNames;
        //    }
        //    Yaku saveYaku = new Yaku()
        //    {
        //        score = yaku.score,
        //        yakuName = yaku.yakuName,
        //        presents = new List<Present>(),
        //        presentNames = presentNames
        //    };
        //    for (int i = 0; i < 3; i++)
        //    {
        //        saveYaku.presents.Add(Resources.Load<Present>("Prefabs/Object/Present/" + presentNames[i]));

        //    }

        //    saveYakus.Add(saveYaku);
        //}
       
       
        //string json = "[";
        //foreach (Yaku yaku in saveYakus)
        //{
        //    json += JsonUtility.ToJson(yaku) + "\n";
        //}
        //json += "]";
        //File.WriteAllText("Assets\\Resources\\Data\\YakuList.json", json);
    }
}
