using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using SimpleJSON;
using UniRx;


public class ERankingEntity
{
    public int id;
    public string name;
    public int score;
}

public class RankingTableOperator : EditorWindow
{

    List<ERankingEntity> entities = new List<ERankingEntity>();
    Sprite[] images;
    String[] urlStrings;
    static RankingTableOperator skillTableOperator;
    Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Window/TwitterGame/SkillTableOperator")]
    static void Open()
    {
        if (skillTableOperator == null)
        {
            skillTableOperator = CreateInstance<RankingTableOperator>();
        }
        skillTableOperator.ShowUtility();
    }
    void OnEnable()
    {
        entities = new List<ERankingEntity>();
        Observable.FromCoroutine(GetFollowerData).Subscribe(_ =>
        {
            Repaint();
        });
    }
    void OnGUI()
    {

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);


        float skillFieldWidth = 70;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("ID", GUILayout.Width(30));
        EditorGUILayout.LabelField("名前", GUILayout.Width(100));
        EditorGUILayout.LabelField("スコア", GUILayout.Width(50));

        EditorGUILayout.EndHorizontal();
        foreach (ERankingEntity entity in entities)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(entity.id.ToString(), GUILayout.Width(30));
            entity.name = EditorGUILayout.TextField(entity.name, GUILayout.Width(100));
            entity.score = EditorGUILayout.IntField(entity.score, GUILayout.Width(50));


            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Button("追加"))
        {
            Observable.FromCoroutine(CreateFollowerData).Subscribe(_ =>
            {
            });
        }

        if (GUILayout.Button("全削除"))
        {
            Observable.FromCoroutine(DeleteAllFollowerData).Subscribe(_ =>
            {
            });
        }


        if (GUILayout.Button("保存"))
        {
            Observable.FromCoroutine(SaveFollowerData).Subscribe(_ =>
            {
            });
        }

        EditorGUILayout.EndHorizontal();
    }

    string url = "http://tsuyomilog.php.xdomain.jp/get_follower.php";

    IEnumerator GetFollowerData()
    {
        string sql = "select * from skill";
        WWWForm form = new WWWForm();
        form.AddField("sql", sql);
        WWW www = new WWW(UrlConsts.execute, form.data);

        yield return www;

        var resultJson = JSON.Parse(www.text);
        if (resultJson != null)
        {
            SetFollowerData(resultJson);
        }


    }

    void SetFollowerData(JSONNode resultJson)
    {
        for (int i = 0; i < resultJson.Count; i++)
        {
            var tempEntity = resultJson[i];


            if (tempEntity["skill_name"].ToString() == "") { continue; }
            ERankingEntity entity = new ERankingEntity()
            {
                name = tempEntity["skill_name"],
                score = tempEntity["score"],
                id = int.Parse(tempEntity["id"]),

            };

            entities.Add(entity);

        }
    }

    IEnumerator SaveFollowerData()
    {
        foreach (ERankingEntity entity in entities)
        {
            string sql = "UPDATE skill SET name = '" + entity.name +
            "',score = '" + entity.score +

            "' WHERE id = " + entity.id + ";";
            WWWForm form = new WWWForm();
            form.AddField("sql", sql);
            WWW www = new WWW(UrlConsts.execute, form.data);
            yield return www;
            Debug.Log(sql);
            Debug.Log(www.text);
        }

    }
    IEnumerator CreateFollowerData()
    {
        string sql = "insert into skill " +
                 "(skill_name, description, attribute, power, charge_time, after_attack_time)" +
                "values " +
                  "('', '', 0, 5, 1, 0);";

        WWWForm form = new WWWForm();
        form.AddField("sql", sql);
        WWW www = new WWW(UrlConsts.execute, form.data);

        yield return www;
        Debug.Log(www.text);
        entities = new List<ERankingEntity>();
        Observable.FromCoroutine(GetFollowerData).Subscribe(_ =>
        {
            Repaint();
        });
    }


    IEnumerator DeleteAllFollowerData()
    {


        string sql = "delete from skill";
        WWWForm form = new WWWForm();
        form.AddField("sql", sql);
        WWW www = new WWW(UrlConsts.execute, form.data);

        yield return www;

        Debug.Log(www.text);
        Observable.FromCoroutine(GetFollowerData).Subscribe(_ =>
        {
            Repaint();
        });
    }
}
