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
    Wait,
    Method
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
                        float a;
                        if (float.TryParse(cmd[2], out a)) {
                            GameObject.Find("SnowmanOp").GetComponent<Animator>().speed = float.Parse(cmd[2]);

                        } else
                        {
                            GameObject.Find("SnowmanOp").GetComponent<Animator>().SetTrigger(cmd[2]);
                        }
                        break;
                    case "Signboard":
                        GameObject.Find("Signboard").GetComponent<Animator>().SetTrigger(cmd[2]);
                        break;
                }
            }
        });
        InitializeCommands();
        //StartOpening();
    }
    void InitializeCommands()
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
        commands.Add(new OpeningCommand(OpeningCommandType.Timeline, "OpeningCamera"));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "2"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "2"));

        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages2));

        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages3));
        commands.Add(new OpeningCommand(OpeningCommandType.Timeline, "TaubeFallAnime"));  
        commands.Add(new OpeningCommand(OpeningCommandType.Method, "HideTaubeStar"));  
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "1"));
        
        commands.Add(new OpeningCommand(OpeningCommandType.Message, "@Anim Tim Surprised"));
        commands.Add(new OpeningCommand(OpeningCommandType.Wait, "3"));

        commands.Add(new OpeningCommand(OpeningCommandType.Method, "ShakeTaube"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, "@Anim SnowmanOp Flyout"));

        commands.Add(new OpeningCommand(OpeningCommandType.Input, "TimSuprisedBySnowman"));
        commands.Add(new OpeningCommand(OpeningCommandType.Message, messages4, OpeningCommandMode.Through));
        commands.Add(new OpeningCommand(OpeningCommandType.Timeline, "TimRunAway"));  
        commands.Add(new OpeningCommand(OpeningCommandType.Message, "@Anim Signboard Rolling"));
        commands.Add(new OpeningCommand(OpeningCommandType.Method, "StartSignboardAnime"));
    }
    public void StartOpening()
    {


        NextAction();
    }

    void HideTaubeStar()
    {
        GameObject.Find("TaubeStar").SetActive(false);
                NextAction();
    }
    public void StartTaubeFallAnime()
    {
        StartCoroutine(TaubeFallAnime());
    }
    IEnumerator TaubeFallAnime()
    {
        yield return null;
    }

    void ShakeTaube()
    {
        StartCoroutine(IShakeTaube());
    }

    IEnumerator IShakeTaube()
    {
        GameObject taubeObj = GameObject.Find("SnowmanOp");
        Vector3 initPos = taubeObj.transform.position;
        float shakeRange = 0.1f;
        float interval = 0.05f;
        for(int i = 0; i < 1 / interval; i++)
        {
            taubeObj.transform.position = 
                initPos + new Vector3(UnityEngine.Random.Range(-shakeRange, shakeRange),
                                        UnityEngine.Random.Range(-shakeRange, shakeRange));
            yield return new WaitForSeconds(interval);
        }
        NextAction();
    }



    public void StartSignboardAnime()
    {
        StartCoroutine(SignboardAnime());
    }
    IEnumerator SignboardAnime()
    {
        Animator anime = GameObject.Find("Signboard").GetComponent<Animator>();
        
        anime.speed = 3;
        yield return new WaitForSeconds(1);
        float v = 0.01f;
        while(true)
        {
            v += 0.015f;
            anime.speed -= v;
            if(anime.speed < 0.5f)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        anime.speed = 1;
        anime.SetTrigger("Stop");
        GameObject taubeObj = GameObject.FindGameObjectWithTag("Player");
        taubeObj.GetComponent<PlayerController>().activeSts = true;
        yield return new WaitForSeconds(0.5f);
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
            case OpeningCommandType.Method:
                Invoke(command.msg[0], 0);
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

                    Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(a =>
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
