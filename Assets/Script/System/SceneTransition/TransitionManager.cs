using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using Tsuyomi.Yukihuru.Scripts.Utilities.SceneDataPacks;

namespace Tsuyomi.Yukihuru.Scripts.Utilities.Transition
{
    public class TransitionManager : SingletonMonoBehaviourFast<TransitionManager>
    {

        private bool _isRunning = false;

        public bool IsRunning { get { return _isRunning; } }

        Fade fade = null;

        private GameScenes _currentGameScene;

        public GameScenes CurrentGameScene
        {
            get { return _currentGameScene; }
        }

        private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();
        public IObservable<Unit> OnScenesLoaded { get { return onAllSceneLoaded; } }


        private Subject<Unit> _onTransitionAnimationFinishedSubject = new Subject<Unit>();

        public IObservable<Unit> OnTransitionAnimationFinished
        {
            get
            {
                if(_isRunning)
                {
                    return _onTransitionAnimationFinishedSubject.FirstOrDefault();
                } else
                {
                    return Observable.Return(Unit.Default);
                }
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            try
            {
                _currentGameScene = (GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
            }
            catch
            {
                Debug.Log("現在のシーンの取得に失敗");
                _currentGameScene = GameScenes.Title;
            }
        }

        public void StartTransaction(GameScenes nextScene, SceneDataPack data, GameScenes[] additiveLoadScenes, bool autoMove)
        {
            if (_isRunning) return;
            StartCoroutine(TransitionCoroutine(nextScene, data, additiveLoadScenes, autoMove));
        }

        private IEnumerator TransitionCoroutine(GameScenes nextScene, SceneDataPack data, GameScenes[] additiveLoadScenes, bool autoMove)
        {
            _isRunning = true;
            fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();
            fade.FadeIn(1, null);
            yield return new WaitForSeconds(1);
            yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

            if(additiveLoadScenes != null)
            {
                yield return additiveLoadScenes.Select(scene => SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive).AsObservable()).WhenAll().ToYieldInstruction();
            }

            yield return null;

            Resources.UnloadUnusedAssets();
            GC.Collect();

            yield return null;

            _currentGameScene = nextScene;
            fade.FadeOut(1, null);
            yield return new WaitForSeconds(1);

            onAllSceneLoaded.OnNext(Unit.Default);


            _onTransitionAnimationFinishedSubject.OnNext(Unit.Default);

            _isRunning = false;
        }
    }
}