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

struct OpeningCommand
{
    public OpeningCommandType type;
    public List<string> msg;

    public OpeningCommand(OpeningCommandType _type, List<string> _msg)
    {
        type = _type;
        msg = _msg;
    }

    public OpeningCommand(OpeningCommandType _type, string _msg)
    {
        type = _type;
        List<string> tmp = new List<string>();
        tmp.Add(_msg);
        msg = tmp;
    }

    public OpeningCommand(OpeningCommandType _type)
    {
        type = _type;
        msg = new List<string>();
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
                if(cmd[1] == "Tim")
                {
                    switch(cmd[2])
                    {
                        case "LookUp":
                            timAnim.SetTrigger("LookUp");
                            break;
                    }
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
        messages2.Add("クリスマスは明日だっていうのに、僕だけじゃプレゼントなんて作れないよ…！");

        messages2.Add("@Face Tim");

        List<string> messages3 = new List<string>();
        messages3.Add("@Face Tim");


        Pauser.Pause();
        commands.Add(new OpeningCommand(OpeningCommandType.Timeline, "TimHitsSnowman"));

        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "2"));

        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages2));

        //commands.Add(new OpeningCommand(OpeningCommandType.Message, messages3));
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
                messageWindowController.StartMessage(command.msg);
                messageWindowController.OnMessageFinished.First().Subscribe(_ =>
                {
                    NextAction();
                });
                break;
            case OpeningCommandType.Timeline:
                PlayableDirector playableDirector = GameObject.Find(command.msg[0]).GetComponent<PlayableDirector>();
                playableDirector.stopped += OnTimelineStopped;
                playableDirector.Play();
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
        }
    }

    public void NextAction()
    {
        actionCount++;
        if (commands.Count - 1 >= actionCount)
        {
            PlayCommand(commands[actionCount]);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
