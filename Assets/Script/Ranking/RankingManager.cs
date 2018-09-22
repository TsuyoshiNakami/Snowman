using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UniRx;
using System;
using TMPro;

public class RankingEntity
{
    public int id;
    public string name;
    public int score;
}

public class RankingManager : MonoBehaviour {

    List<RankingEntity> entities = new List<RankingEntity>();


    Subject<Unit> loadRankingSubject = new Subject<Unit>();
    public IObservable<Unit> OnLoadRanking 
    {
        get { return loadRankingSubject; }
    }

    [SerializeField] RankingSender rankingSender;
    [SerializeField] GameObject rankingWindow;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject rankingRowObject;

    // Use this for initialization
    void Start () {
        if(rankingSender == null)
        {
            return;
        }
        rankingSender.OnClickSendButton.Subscribe(_ =>
        {
            SendData();
        });
        //rankingSender.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R))
        {
            ShowRankingSender();
        }
	}

    void ShowRankingSender()
    {
        GameObject tmp = rankingSender.transform.parent.gameObject;
        rankingSender.SetScoreText(PresentGameManager.score);
            tmp.SetActive(!tmp.activeInHierarchy);
    }

    void SendData()
    {
        Observable.FromCoroutine(ESaveRanking).Subscribe(_ =>
        {
            ShowRanking();
        });

        GameObject tmp = rankingSender.transform.parent.gameObject;
        tmp.SetActive(false);
    }


    IEnumerator GetRanking()
    {
        string sql = "select * from ranking";
        WWWForm form = new WWWForm();
        form.AddField("sql", sql);
        WWW www = new WWW(PresentGameConsts.execute, form.data);

        yield return www;

        var resultJson = JSON.Parse(www.text);
        Debug.Log(www.text);
        if (resultJson != null)
        {
            SetRankingData(resultJson);
        }

        loadRankingSubject.OnNext(Unit.Default);
    }

    void SetRankingData(JSONNode resultJson)
    {

        for (int i = 0; i < resultJson.Count; i++)
        {
            var tempEntity = resultJson[i];


            if (tempEntity["name"].ToString() == "") { continue; }
            RankingEntity entity = new RankingEntity()
            {
                id = tempEntity["id"],
                name = tempEntity["name"],
                score = int.Parse(tempEntity["score"]),
            };
            entities.Add(entity);
        }
    }

    public void ShowRanking()
    {
        foreach(Transform row in rankingWindow.transform)
        {
            Destroy(row.gameObject);
            entities.Clear();
        } 
        Observable.FromCoroutine(GetRanking).Subscribe(_ =>
        {
            rankingWindow.SetActive(true);
            entities.Sort((a, b) => b.score - a.score);
            for (int i = 0; i < entities.Count; i++)
            {
                if (i < 8)
                {
                    AddRankingRow(i, entities[i]);
                } else
                {
                    DeleteRowData(entities[i].id);
                }
            }
        });
    }

    public void HideRanking()
    {
        rankingWindow.SetActive(false);
    }

    void AddRankingRow(int i, RankingEntity entity)
    {
        GameObject newObj = Instantiate(rankingRowObject, rankingWindow.transform);
        RankingRow row = newObj.GetComponent<RankingRow>();
        row.Initialize();
        row.SetText(i, entity.name, entity.score);
    }

    void DeleteRowData(int id)
    {
        string sql = "DELETE FROM ranking where id = " + id;
    }
    IEnumerator ESaveRanking()
    {
        string playerName = nameText.text;
        string sql = "INSERT INTO ranking (name, score) VALUES('" + playerName +
        "'," + PresentGameManager.score + ")";
        
        Debug.Log(sql);
        WWWForm form = new WWWForm();
            form.AddField("sql", sql);
            WWW www = new WWW(PresentGameConsts.execute, form.data);
            yield return www;
            Debug.Log(www.text);
    }
}
