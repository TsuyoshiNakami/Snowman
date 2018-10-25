using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UniRx;
using System;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField] bool enableKeyInput = true;
    [SerializeField] int rankingRowMax = 8;
    int minRankingScore = 0;
    bool canSendRanking = false;
    public static bool hasSendRanking = false;
    // Use this for initialization
    void Start () {
        Debug.Log("RankingManager start");
        if(rankingSender == null)
        {
            Observable.FromCoroutine(GetRanking).Subscribe(_ =>
        {
            SortAndCutRankingRow();
            ShowRanking();
        });
            return;
        }
        SetActiveRankingSender(false);
                    rankingWindow.SetActive(false);
        Observable.FromCoroutine(GetRanking).Subscribe(_ =>
        {
            SortAndCutRankingRow();
            canSendRanking = minRankingScore < PresentGameManager.score ? true : false;
            if (canSendRanking && !hasSendRanking)
            {
                Debug.Log("なんで？");
                rankingSender.SetScoreText(PresentGameManager.score);
                SetActiveRankingSender(true);
            } else
            {
                SetActiveRankingSender(false);
                ShowRanking();
            }
        });

        rankingSender.OnClickSendButton.Subscribe(_ =>
        {
            SendData();
        });


        //rankingSender.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!enableKeyInput) return;
        if(Input.GetButtonDown(KeyConfig.Cancel))
        {
            GameManager.LoadScene(GameScenes.Title);
        }

	}

    public void SetActiveRankingSender(bool f)
    {
        GameObject tmp = rankingSender.transform.parent.parent.gameObject;

            tmp.SetActive(f);
        if(f)
        {
            rankingSender.GetComponent<Button>().interactable = true;
        }
    }

    public void OnClickDontSendButton()
    {
        SetActiveRankingSender(false);
        ShowRanking();
    }
    void SendData()
    {
        Observable.FromCoroutine(ESaveRanking).Subscribe(_ =>
        {
            Debug.Log("asd:fjop");
            hasSendRanking = true;
            RankingEntity entity = new RankingEntity();
            entity.name = nameText.text;
            entity.score = PresentGameManager.score;
            entities.Add(entity);
            SortAndCutRankingRow();
            ShowRanking();
        });

        SetActiveRankingSender(false);
    }


    IEnumerator GetRanking()
    {
        string sql = "select * from ranking";
        WWWForm form = new WWWForm();
        form.AddField("sql", sql);
        WWW www = new WWW(PresentGameConsts.execute, form.data);
                    SortAndCutRankingRow();
        yield return www;

        var resultJson = JSON.Parse(www.text);
        //Debug.Log(www.text);
        if (resultJson != null)
        {
            SetRankingData(resultJson);
        }

        loadRankingSubject.OnNext(Unit.Default);
    }

    void SetRankingData(JSONNode resultJson)
    {
        foreach(Transform row in rankingWindow.transform)
        {
            Destroy(row.gameObject);
        }
            entities.Clear();

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

            rankingWindow.SetActive(true);

    }

    public void SortAndCutRankingRow()
    {

        foreach (Transform row in rankingWindow.transform)
        {
            Destroy(row.gameObject);
        }
            entities.Sort((a, b) => b.score - a.score);
            for (int i = 0; i < entities.Count; i++)
            {
                if (i < rankingRowMax)
                {
                    AddRankingRow(i, entities[i]);
                    if(i == rankingRowMax - 1)
                    {
                        minRankingScore = entities[i].score;

                    }
                } else
                {
                    DeleteRowData(entities[i].id);
                }
            }
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
