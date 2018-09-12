using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class RankingSender : MonoBehaviour {

    Subject<Unit> clickSendButtonSubject = new Subject<Unit>();
    public IObservable<Unit> OnClickSendButton
    {
        get { return clickSendButtonSubject; }
    }
    [SerializeField] TextMeshProUGUI scoreText;

    // Use this for initialization
    void Start () {
		
	}
	
    public void SetScoreText(int score)
    {
        scoreText.text = score + "てん";
    }
	public void Send()
    {
        clickSendButtonSubject.OnNext(Unit.Default);
    }
}
