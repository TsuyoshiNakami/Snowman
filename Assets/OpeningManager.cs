using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UniRx;
using System;
using Tsuyomi.Yukihuru.Scripts.Utilities;

enum OpeningCommandType
{
    Message,
    Timeline,
    Input,
    ToGame,
    Wait
}

enum OpeningCommandMode
{
    Wait,
    Through
}
struct OpeningCommand
{
    public OpeningCommandType type;
    public List<string> msg;
    public OpeningCommandMode mode;

    public OpeningCommand(OpeningCommandType _type, List<string> _msg)
    {
        type = _type;
        msg = _msg;
        mode = OpeningCommandMode.Wait;
    }
    public OpeningCommand(OpeningCommandType _type, List<string> _msg, OpeningCommandMode _mode)
    {
        type = _type;
        msg = _msg;
        mode = _mode;
    }


    public OpeningCommand(OpeningCommandType _type, string _msg)
    {
        type = _type;
        List<string> tmp = new List<string>();
        tmp.Add(_msg);
        msg = tmp;
        mode = OpeningCommandMode.Wait;
    }
    public OpeningCommand(OpeningCommandType _type, string _msg, OpeningCommandMode _mode)
    {
        type = _type;
        List<string> tmp = new List<string>();
        tmp.Add(_msg);
        msg = tmp;
        mode = _mode;
    }
    public OpeningCommand(OpeningCommandType _type)
    {
        type = _type;
        msg = new List<string>();
        mode = OpeningCommandMode.Wait;
    }
}

public class OpeningManager : MonoBehaviour
{
    [SerializeField] MessageWindowController messageWindowController;
    [SerializeField] List<Playable> playables;
    Animator timAnim;
    int actionCount = -1;
    List<OpeningCommand> commands = new List<OpeningCommand>();

    private void Start()
    {
        timAnim = GameObject.Find("Tim").GetComponent<Animator>();
        messageWindowController.autoScroll = true;

        messageWindowController.OnReceiveCommand.Subscribe(cmd =>
        {
            if(cmd[0] == "Anim")
            {
                switch(cmd[1])
                {
                    case "Tim":
                    timAnim.SetTrigger(cmd[2]);
                        break;
                    case "SnowmanOp":
                        Debug.Log("Anime SnowmanOP ");
                        GameObject.Find("SnowmanOp").GetComponent<Animator>().SetTrigger(cmd[2]);
                        break;
                    case "Signboard":
                        GameObject.Find("Signboard").GetComponent<Animator>().SetTrigger(cmd[2]);
                        break;
                }
            }
        });

        StartOpening();
    }
    public void StartOpening()
    {
        List<string> messages = new List<string>();
        messages.Add("@Face Tim");
        messages.Add("マスター、どこいっちゃったの…");
        messages.Add("@Anim Tim LookUp");

        List<string> messages2 = new List<string>();
        messages2.Add("@Anim Tim LookUpSpeak");
        messages2.Add("クリスマスは明日だっていうのに、僕だけじゃプレゼントなんて作れないよ…！");


        List<string> messages3 = new List<string>();
        messages3.Add("@Anim Tim Worry");
        messages3.Add("はあ・・・もうどうしよう・・・？");

        List<string> messages4 = new List<string>();
        messages4.Add("@Anim Tim RunAway");
        messages4.Add("うわあああ！");


        Pauser.Pause();
        //commands.Add(new OpeningCommand(OpeningCommandType.Timeline, "TimHitsSnowman"));

        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "0.5"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "2"));

        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages2));

        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages3));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "2"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, "@Anim Tim Surprised"));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "2"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, "@Anim SnowmanOp Flyout"));

        commands.Add(new OpeningCommand(OpeningCommandType.Input, "TimSuprisedBySnowman"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages4, OpeningCommandMode.Through));
        commands.Add(new OpeningCommand(OpeningCommandType.Timeline, "TimRunAway"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, "@Anim Signboard Rolling"));

        NextAction();
    }

    void PlayCommand(OpeningCommand command)
    {
        switch (command.type)
        {
            case OpeningCommandType.Input:
                DetectInputCommand(command.msg);
                break;
            case OpeningCommandType.Message:
                if (command.mode == OpeningCommandMode.Through)
                {
                    NextAction();
                }
                else
                {
                    messageWindowController.OnMessageFinished.First().Subscribe(_ =>
                    {
                        NextAction();
                    });
                }
                messageWindowController.StartMessage(command.msg);
                break;
            case OpeningCommandType.Timeline:
                PlayableDirector playableDirector = GameObject.Find(command.msg[0]).GetComponent<PlayableDirector>();
                playableDirector.Play();
                if (command.mode == OpeningCommandMode.Through)
                {
                    NextAction();
                }
                else
                {
                    playableDirector.stopped += OnTimelineStopped;
                }
                break;
            case OpeningCommandType.ToGame:
                ES3.Save<bool>("Opening", true, PresentGameConsts.saveSetting);
                SceneLoader.LoadScene(GameScenes.GameEasy);

                break;
            case OpeningCommandType.Wait:
                Invoke("NextAction", float.Parse(command.msg[0]));
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
        switch (msg[0])
        {
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
            case "TimSuprisedBySnowman":
                GameObject.Find("SnowmanOp").GetComponent<OpeningSnowman>().OnTaubeAppear.First().Subscribe(_ =>
                {

                    Observable.Timer(TimeSpan.FromSeconds(0.5f)).Subscribe(a =>
                   {
                       NextAction();

                   });
                });
                break;
        }
    }

    public void NextAction()
    {
        actionCount++;
        if (commands.Count - 1 >= actionCount)
        {
            Debug.Log("PlayAction : " + commands[actionCount].type.ToString() + ", " + commands[actionCount].msg[0]);
            PlayCommand(commands[actionCount]);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
