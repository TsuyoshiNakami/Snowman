//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SimpleJSON;
//using TwitterKit.Unity;

//public class TweetHandler : MonoBehaviour {

//    //アプリのコンシューマーキー
//    private const string CONSUMER_KEY = "aAraeC2FIc9zzO1Q1OLNCle7Z";

//    //アプリのコンシューマーシークレット
//    private const string CONSUMER_SECRET = "EFJ31oHpLoamlbcIHhWbxoOG88fkGSOkB5AcWTm92bfFi01WoM";
//    private string _AccessToken;
//    private string _Secret;
//    TwitterSession session;
//    LetsTwitter.RequestTokenResponse m_RequestTokenResponse;
//    // Use this for initialization
//    void Start () {
//        this.TwitterAuth();


//        //今回はScreenShot.pngの画像をツイートするものとします。
//        string filename = "ScreenShot.png";

//        //ツイートしたい文章です。上の画像のテキストフィールドのテキストが入ります。
//        string myTweet = "てすと";


//        //まず　ツイート文章がからでない　かつ　空白文字のみでない場合のみツイートメソッドを呼び出します。
//        if (!(string.IsNullOrEmpty(myTweet)) &&  !(myTweet.Trim() == "")){


//                //画像用ツイートメソッドを呼び出していま。
//           //     StartCoroutine(Twitter.API.UploadPic(myTweet, filename, CONSUMER_KEY, CONSUMER_SECRET, response,
//             //   new Twitter.UploadPicCallback(this.OnUploadPic)));
//                //ツイートが終わったらScreenShot.pngを削除しています。
//                //これがないと毎回この画像と一緒にツイートしてしまいます。
//                System.IO.File.Delete(@"ScreenShot.png");

//        } 
//    }
//    public void TwitterAuth()
//    {
//        StartCoroutine(LetsTwitter.API.GetRequestToken(CONSUMER_KEY, CONSUMER_SECRET,
//        new LetsTwitter.RequestTokenCallback(this.OnRequestTokenCallback)));
//    }
//    void OnRequestTokenCallback(bool success, LetsTwitter.RequestTokenResponse response)
//    {
//        if (success)
//        {
//            string log = "OnRequestTokenCallback - succeeded";
//            log += "\n    Token : " + response.Token;
//            log += "\n    TokenSecret : " + response.TokenSecret;
//            print(log);

//            m_RequestTokenResponse = response;


//            LetsTwitter.API.OpenAuthorizationPage(response.Token);
//        }
//        else
//        {
//            print("OnRequestTokenCallback - failed.");
//        }
//    }
//    public void LoginComplete(TwitterSession session)
//    {
//        Debug.Log("[Info] : Login success. " + session.authToken);
//        _AccessToken = session.authToken.token;
//        _Secret = session.authToken.secret;
//        this.session = session;
//        StartCoroutine(CaptureScreen());
//    }

//    public void LoginFailure(ApiError error)
//    {
//        Debug.Log("[Error ] : Login faild code =" + error.code + " msg =" + error.message);
//    }

//    public void StartCompose(TwitterSession session, string imageUri)
//    {
//        Debug.Log("start compose");
//        Twitter.Compose(session, imageUri, "テスト", null,
//                (string tweetId) => { UnityEngine.Debug.Log("Tweet Success, tweetId = " + tweetId); },
//                (ApiError error) => { UnityEngine.Debug.Log("Tweet Failed " + error.message); },
//                () => { Debug.Log("Compose cancelled"); }
//        );
//    }


//    IEnumerator CaptureScreen()
//    {
//        ScreenCapture.CaptureScreenshot("Assets/StreamingAssets/image.png");
//        yield return null;
//        Debug.Log("スクリーンショット撮れた");
//        // StartCompose(session, Application.dataPath + "/StreamingAssets");
//        LetsTwitter.AccessTokenResponse response = new LetsTwitter.AccessTokenResponse();
//        response.UserId = session.id.ToString();
//        response.ScreenName = session.userName;
//        response.Token = "729944016507166720-ENfdAEuV4dumz8WDxDBRjZvV7TgAimK";// _AccessToken;
//        response.TokenSecret = "ILrRZ7LnH7jlIpMYgSKYahEOI4utXufNHiZJofsgCqWpV";//_Secret;
//        Debug.Log("AccessToken : " + _AccessToken + "\nTokenSecret : " + _Secret);
//        StartCoroutine(LetsTwitter.API.PostTweetWithMedia("テスト！", Application.dataPath + "/StreamingAssets/image.png", CONSUMER_KEY, CONSUMER_SECRET, response, new LetsTwitter.PostTweetCallback(this.OnPostTweet)));
//        //StartCoroutine(LetsTwitter.API.PostTweet("テスト", CONSUMER_KEY, CONSUMER_SECRET, response, new LetsTwitter.PostTweetCallback(this.OnPostTweet)));

//    }   // Update is called once per frame

//    void OnPostTweet(bool success)
//    {
//        if(success)
//        {
//            Debug.Log("Tweet Completed");
//        } else
//        {
//            Debug.Log("失敗だよーん");
//        }
//    }
//}
