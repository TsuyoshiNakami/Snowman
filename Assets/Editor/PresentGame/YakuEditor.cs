using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;
using SimpleJSON;
using System.IO;
using System.Text;


public class YakuEditor : EditorWindow
{
    static Present[] presents;
    static YakuEditor yakuEditor;
    static List<Yaku> yakus = new List<Yaku>();
    static Dictionary<Yaku, List<string>> presentNames = new Dictionary<Yaku, List<string>>();
    string sql = "";
    [MenuItem("Window/PresentGame/YakuEditor %#e")]
    static void Open()
    {
        if(yakuEditor == null)
        {
            yakuEditor = CreateInstance<YakuEditor>();
        }
        
        yakus.Clear();
        presentNames.Clear();
        StreamReader streamReader = new StreamReader("Assets\\Resources\\Data\\YakuList.json",
            Encoding.GetEncoding("UTF-8"));
        yakus = JsonToYaku(streamReader.ReadToEnd());
        yakuEditor.ShowUtility();
        streamReader.Dispose();
    }

    static List<Yaku> JsonToYaku(string jsonStr)
    {

        var json = JSON.Parse(jsonStr);
        List<Yaku> entities = new List<Yaku>();

        for (int i = 0; i < json.Count; i++)
        {
            var tempEntity = json[i];
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
            entities.Add(entity);

        }
        return entities;
    }

    void OnGUI()
    {
        Yaku deleteYaku = null;

        presents = Resources.LoadAll<Present>("Prefabs/Object/Present");
        //================================= 各項目の表示（プレゼント1色） ================================

        foreach (Present present in presents) 
        {

            if(present.completeYaku.yakuName == "")
            {
                continue;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(present.presentName, GUILayout.Width(160));
            present.completeYaku.yakuName = EditorGUILayout.TextField(present.completeYaku.yakuName, GUILayout.Width(160));
            present.completeYaku.score = EditorGUILayout.IntField(present.completeYaku.score, GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();
        }

        //================================= 各項目の表示（バラエティ） ================================

        EditorGUILayout.Space();
        foreach (Yaku entity in yakus)
        {
            EditorGUILayout.BeginHorizontal();
            entity.yakuName = EditorGUILayout.TextField(entity.yakuName, GUILayout.Width(160));
            entity.score = EditorGUILayout.IntField(entity.score, GUILayout.Width(50));
            for(int i = 0; i < 3; i++) {

                presentNames[entity][i] = EditorGUILayout.TextField(presentNames[entity][i]);
            }
            if (GUILayout.Button("削除"))
            {
                deleteYaku = entity;

            }
                EditorGUILayout.EndHorizontal();
        }


        if(deleteYaku != null)
        {
            yakus.Remove(deleteYaku);
            presentNames.Remove(deleteYaku);
        }


        if (GUILayout.Button("追加"))
        {
            Yaku yaku = new Yaku()
            {

            };
            List<string> presentNameList = new List<string>();
            for (int m = 0; m < 3; m++)
            {
                string presentName = "";
                presentNameList.Add(presentName);
            }
            presentNames.Add(yaku, presentNameList);
            yakus.Add(yaku);
        }
            if (GUILayout.Button("保存"))
        {


            List<Yaku> saveYakus = new List<Yaku>();
            foreach(Yaku yaku in yakus)
            {
                Yaku saveYaku = new Yaku()
                {
                    score = yaku.score,
                    yakuName = yaku.yakuName,
                    presents = new List<Present>(),
                    presentNames = presentNames[yaku]
                };
                for (int i = 0; i < 3; i++)
                {
                    saveYaku.presents.Add(Resources.Load<Present>("Prefabs/Object/Present/" + presentNames[yaku][i]));
                    
                }

                saveYakus.Add(saveYaku);
            }

            GameObject prefab = Resources.Load<GameObject>("Prefabs/System/YakuList");
            GameObject newPrefab = new GameObject();
            YakuList newYakus = newPrefab.AddComponent<YakuList>();
            newYakus.yakus = saveYakus;
            newYakus.defaultYaku = prefab.GetComponent<YakuList>().defaultYaku;

            PrefabUtility.ReplacePrefab(newPrefab, prefab);
            DestroyImmediate(newPrefab);
            string json = "[";
            foreach (Yaku yaku in saveYakus)
            {
                json += JsonUtility.ToJson(yaku) + "\n";
            }
            json += "]";
            File.WriteAllText("Assets\\Resources\\Data\\YakuList.json", json);
        }

    }


}
