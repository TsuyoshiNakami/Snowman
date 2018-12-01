using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;
using SimpleJSON;
using System.IO;
using System.Text;


public class YukihuruSettings : EditorWindow
{
    static Present[] presents;
    static YukihuruSettings yukihuruSettings;
    static List<Yaku> yakus = new List<Yaku>();
    static Dictionary<Yaku, List<string>> presentNames = new Dictionary<Yaku, List<string>>();
    static PresentGameManager presentGameManager;
    static PresentGameDirectorEasy presentGameDirector;
    string sql = "";
    [MenuItem("Window/PresentGame/YukihuruSettings #&s")]
    static void Open()
    {
        if(yukihuruSettings == null)
        {
            yukihuruSettings = CreateInstance<YukihuruSettings>();
        }
        presentGameManager = GameObject.Find("SceneContext").GetComponent<PresentGameManager>();
        presentGameDirector = GameObject.Find("SceneContext").GetComponent<PresentGameDirectorEasy>();
        yakus.Clear();
        presentNames.Clear();
        StreamReader streamReader = new StreamReader("Assets\\Resources\\Data\\YakuList.json",
            Encoding.GetEncoding("UTF-8"));
        yakus = JsonToYaku(streamReader.ReadToEnd());
        yukihuruSettings.ShowUtility();
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
        //================================= Present Game Manager ================================


            EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("制限時間", GUILayout.Width(160));
        EditorGUILayout.LabelField("プレゼント放出間隔", GUILayout.Width(160));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
            presentGameManager.initialTimeLimit = EditorGUILayout.FloatField(presentGameManager.initialTimeLimit, GUILayout.Width(50));
            presentGameDirector.generateInterval = EditorGUILayout.FloatField(presentGameDirector.generateInterval, GUILayout.Width(50));
        EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();



        EditorGUILayout.Space();

        //    if (GUILayout.Button("保存"))
        //{
        //    OnClickSaveButton();
        //}

    }

    void OnClickSaveButton()
    {

            // 保存するデータをテキストフィールドから取得
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

            // 既存のプレハブと置き換えるためのゲームオブジェクトを生成
            GameObject prefab = Resources.Load<GameObject>("Prefabs/System/YakuList");
            GameObject newPrefab = new GameObject();

            // ゲームオブジェクトのプロパティを設定
            YakuList newYakus = newPrefab.AddComponent<YakuList>();
            newYakus.yakus = saveYakus;
            newYakus.defaultYaku = prefab.GetComponent<YakuList>().defaultYaku;

            //置き換え
            PrefabUtility.ReplacePrefab(newPrefab, prefab);
            DestroyImmediate(newPrefab);

            // セッティングファイルを上書き
            string json = "[";
            foreach (Yaku yaku in saveYakus)
            {
                json += JsonUtility.ToJson(yaku) + "\n";
            }
            json += "]";
            File.WriteAllText("Assets\\Resources\\Data\\YakuList.json", json);
    }

}
