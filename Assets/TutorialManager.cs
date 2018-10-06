using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TutorialManager : MonoBehaviour {
    [SerializeField]MessageWindowController messageWindowController;
    int actionCount = -1;

	// Use this for initialization
	void Start () {
        NextAction();
    }

    void NextAction()
    {
        actionCount++;
        switch (actionCount)
        {
            case 0:
                List<string> messages = new List<string>();
                messages.Add("@Face Tim");
                messages.Add("ねえトーブ、大変だよ！マスターがいなくなっちゃったんだ！");
                messages.Add("クリスマスは明日だっていうのに…プレゼントどうしよう？？" +
                            "ボク、プレゼントなんて作ったことないよ…！");
                messageWindowController.StartMessage(messages);
                messageWindowController.OnMessageFinished.First().Subscribe(_ =>
                {
                    NextAction();
                });
                break;
                // プレゼントを箱に一つ入れたら
            case 1:
                GameObject.Find("Basket").GetComponent<BasketPresentViewer>().OnPresentEnter
                                 .First()
                                .Subscribe(_ =>

                {
                    Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(a =>
                    {
                        List<string> messages2 = new List<string>();
                        messages2.Add("@Face Tim");
                        messages2.Add("あれ？キミ、プレゼントを投げられるのかい？");
                        messages2.Add("じゃあもしかして…プレゼントを箱に3つ入れて完成させることもできるのかい？");
                        messageWindowController.StartMessage(messages2);
                        messageWindowController.OnMessageFinished.First().Subscribe(_2 =>
                        {
                            NextAction();
                        });
                    }).AddTo(this);
                });
        
                break;
                // プレゼントを後2個増やして、プレゼント箱を完成させる
            case 2:

                //プレゼントが完成したら
                GameObject.Find("Basket").GetComponent<BasketPresentViewer>().OnMakeYaku.Subscribe(yaku =>
                {

                    Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(a =>
                   {
                       List<string> messages3 = new List<string>();
                       messages3.Add("@Face Tim");
                       messages3.Add("キミ、すごいよ！プレゼントが完成した！");
                       messages3.Add("この調子でプレゼント作っていけば、クリスマスに間に合うよ！");
                       messageWindowController.StartMessage(messages3);
                       messageWindowController.OnMessageFinished.Subscribe(_2 =>
                       {
                           NextAction();
                       });
                   });
                });
                break;
            case 3:
                GameManager.LoadScene(GameScenes.Game);
                break;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
