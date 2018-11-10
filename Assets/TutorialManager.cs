using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UniRx;
using System;
using Tsuyomi.Yukihuru.Scripts.Utilities;

enum TutorialCommandType{
    Message,
    Timeline,
    Input,
    ToGame
}

struct TutorialCommand
{
    public TutorialCommandType type;
    public List<string> msg;

    public TutorialCommand(TutorialCommandType _type, List<string> _msg)
    {
        type = _type;
        msg = _msg;
    }

    public TutorialCommand(TutorialCommandType _type, string _msg)
    {
        type = _type;
        List<string> tmp = new List<string>();
        tmp.Add(_msg);
        msg = tmp;
    }

    public TutorialCommand(TutorialCommandType _type)
    {
        type = _type;
        msg = new List<string>();
    }
}

public class TutorialManager : MonoBehaviour {
    [SerializeField]MessageWindowController messageWindowController;
    int actionCount = -1;
    List<TutorialCommand> commands = new List<TutorialCommand>();

    public void StartTutorial () {
        List<string> messages = new List<string>();
                messages.Add("@Face Tim");
                messages.Add("マスター！ねえ、マスターってば！");
                
                messages.Add("あれー、ほんとどこいっちゃったんだろう…");
                messages.Add("クリスマスは明日だっていうのに…プレゼントどうしよう？？" +
                            "こんな不器用なボクがプレゼントなんて作れるわけないよ…！");
                messages.Add("雪だるまだって作るのこんな下手くそなのに！");
                List<string> messages2 = new List<string>();
        messages2.Add("@Face Tim");
        messages2.Add("あれ？キミ、プレゼントを投げられるのかい？");
        messages2.Add("じゃあもしかして…プレゼントを箱に3つ入れて完成させることもできるのかい？");
                       List<string> messages3 = new List<string>();
                       messages3.Add("@Face Tim");
                       messages3.Add("キミ、すごいよ！プレゼントが完成した！");
                       messages3.Add("この調子でプレゼント作っていけば、クリスマスに間に合うよ！");

        Pauser.Pause();
        commands.Add(new TutorialCommand(TutorialCommandType.Timeline, messages));

        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages));
        commands.Add(new TutorialCommand(TutorialCommandType.Input, "EnterPresent"));

        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages2));
        commands.Add(new TutorialCommand(TutorialCommandType.Input, "MakePresent"));

        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages3));
        commands.Add(new TutorialCommand(TutorialCommandType.ToGame));
        NextAction();
    }

    /* commandを作る
    
        commandとstringsのDictionary
        コマンドは：
            メッセージ表示
            タイムライン再生
            入力待ち
         
         */

    void PlayCommand(TutorialCommand command)
    {
        switch(command.type)
        {
            case TutorialCommandType.Input:
                DetectInputCommand(command.msg);
                break;
            case TutorialCommandType.Message:
                messageWindowController.StartMessage(command.msg);
                messageWindowController.OnMessageFinished.First().Subscribe(_ =>
                {
                    NextAction();
                });
                break;
            case TutorialCommandType.Timeline:
                PlayableDirector playableDirector = GameObject.Find("Timeline").GetComponent<PlayableDirector>();
                playableDirector.stopped += OnTimelineStopped;
                playableDirector.Play();
                break;
            case TutorialCommandType.ToGame:
                ES3.Save<bool>("Tutorial", true, PresentGameConsts.saveSetting);
                SceneLoader.LoadScene(GameScenes.GameBase, null, new GameScenes[] { GameScenes.GameEasy});

                break;
        }
    }

    void OnTimelineStopped(PlayableDirector playableDirector)
    {

        Destroy(playableDirector);
        NextAction();
        //playableDirector.stopped -= OnTimelineStopped;
    }
    void DetectInputCommand(List<string> msg)
    {
        switch (msg[0]) {
            case "EnterPresent":
                        GameObject.Find("Basket").GetComponent<BasketPresentViewer>().OnPresentEnter
                    .First()
                   .Subscribe(_ =>

                     {
                         Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(a =>
                        {


                            NextAction();
                            ;
                        });
                     });
                break;
            case "MakePresent":
                GameObject.Find("Basket").GetComponent<BasketPresentViewer>().OnMakeYaku.Subscribe(yaku =>
                {

                    Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(a =>
                   {
     
                           NextAction();

                   });
                });
                break;
        }
    }

    void NextAction()
    {
        actionCount++;
        if (commands.Count - 1 >= actionCount)
        {
            PlayCommand(commands[actionCount]);
        }
        /*
        switch (actionCount)
        {
            case 0:
                List<string> messages = new List<string>();
                messages.Add("@Face Tim");
                messages.Add("ねえトーブ、大変だよ！マスターがいなくなっちゃったんだ！");
                messages.Add("クリスマスは明日だっていうのに…プレゼントどうしよう？？" +
                            "ボク、プレゼントなんて作ったことないよ…！");
                messageWindowController.StartMessage(messages);
                break;
                // プレゼントを箱に一つ入れたら
            case 1:
                GameObject.Find("Basket").GetComponent<BasketPresentViewer>().OnPresentEnter
                                 .First()
                                .Subscribe(_ =>

                {
                    Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(a =>
                    {

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
        */
    }
	// Update is called once per frame
	void Update () {
		
	}
}
