﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UniRx;
using System;
using Tsuyomi.Yukihuru.Scripts.Utilities;

enum TutorialCommandType
{
    Message,
    Timeline,
    Input,
    ToGame,
    Wait,
    Pause,
    Resume
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

public class TutorialManager : MonoBehaviour
{
    Animator timAnim;
    [SerializeField] MessageWindowController messageWindowController;
    int actionCount = -1;
    List<TutorialCommand> commands = new List<TutorialCommand>();

    private void Start()
    {
        timAnim = GameObject.Find("Tim").GetComponent<Animator>();

        messageWindowController.OnReceiveCommand.Subscribe(cmd =>
        {
            if (cmd[0] == "Anim")
            {
                switch (cmd[1])
                {
                    case "Tim":
                        if (cmd[2] == "Force")
                        {
                            timAnim.Play(cmd[3]);
                        }
                        else
                        {
                            timAnim.SetTrigger(cmd[2]);
                        }
                        break;
                }
            }
        });

        StartTutorial();
    }
    public void StartTutorial()
    {
        List<string> messages = new List<string>();
        List<string> messages2 = new List<string>();
        List<string> messages3 = new List<string>();
        List<string> messages4 = new List<string>();

        messages.Add("@Face Tim");
        messages.Add("ひえええ…あまり近づかないでよ…");

        messages2.Add("@Anim Tim Force Tim_Tutorial_Turn");

        messages3.Add("@Anim Tim Force Tim_Talk_Worried");
        messages3.Add("・・・あれ？キミ、プレゼントを投げられるのかい？");
        messages3.Add("じゃあもしかして…プレゼントを箱に3つ入れて完成させることもできるのかい？");

        messages4.Add("@Anim Tim Force Tim_Talk_Fine");
        messages4.Add("キミ、すごいよ！プレゼントが完成した！");
        messages4.Add("じゃあ、ボクがお菓子を頑張って作るから、キミはそのお菓子を箱に入れてくれるかな？" +
            "さっきの調子でやれば大丈夫だから！");
        messages4.Add("@Anim Tim Force Tim_Talk_Smile");
        messages4.Add("二人で協力して、クリスマスまでにたくさんのプレゼントを作ろう！");

        StartCoroutine(ChangeBGM());

        Pauser.Pause();
        //commands.Add(new TutorialCommand(TutorialCommandType.Timeline, messages));

        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages));
        commands.Add(new TutorialCommand(TutorialCommandType.Input, "EnterPresent"));

        commands.Add(new TutorialCommand(TutorialCommandType.Pause));
        commands.Add(new TutorialCommand(TutorialCommandType.Wait, "1.5"));
        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages2));
        commands.Add(new TutorialCommand(TutorialCommandType.Wait, "1.5"));
        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages3));
        commands.Add(new TutorialCommand(TutorialCommandType.Input, "MakePresent"));

        commands.Add(new TutorialCommand(TutorialCommandType.Message, messages4));
        commands.Add(new TutorialCommand(TutorialCommandType.ToGame));
        NextAction();
    }

    IEnumerator ChangeBGM()
    {

        SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        AudioSource bgm0 = soundManager.GetAudioSource();
        AudioSource bgm1 = soundManager.GetAudioSource(1);

        bgm0.timeSamples = bgm1.timeSamples;

        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time >= 0.05f)
            {
                time -= 0.05f;
                bgm0.volume += 0.01f;
                bgm1.volume -= 0.01f;
            }
            if(bgm0.volume >= 0.5f)
            {
                bgm1.volume = 0;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;

    }

    void PlayCommand(TutorialCommand command)
    {
        switch (command.type)
        {
            case TutorialCommandType.Input:
                DetectInputCommand(command.msg);
                break;
            case TutorialCommandType.Message:
                messageWindowController.OnMessageFinished.First().Subscribe(_ =>
                {
                    NextAction();
                });
                messageWindowController.StartMessage(command.msg);
                break;
            case TutorialCommandType.Timeline:
                PlayableDirector playableDirector = GameObject.Find("Timeline").GetComponent<PlayableDirector>();
                playableDirector.stopped += OnTimelineStopped;
                playableDirector.Play();
                break;
            case TutorialCommandType.ToGame:
                ES3.Save<bool>("Tutorial", true, PresentGameConsts.saveSetting);
        SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
                soundManager.FadeOut(2);
                SceneLoader.LoadScene(GameScenes.GameEasy);

                break;
            case TutorialCommandType.Wait:
                Invoke("NextAction", float.Parse(command.msg[0]));
                break;
            case TutorialCommandType.Pause:
                Pauser.Pause();
                NextAction();
                break;
            case TutorialCommandType.Resume:
                Pauser.Resume();
                NextAction();
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

    void NextAction()
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
