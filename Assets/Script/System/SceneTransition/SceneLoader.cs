using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Tsuyomi.Yukihuru.Scripts.Utilities.SceneDataPacks;
using Tsuyomi.Yukihuru.Scripts.Utilities.Transition;
using UniRx;

namespace Tsuyomi.Yukihuru.Scripts.Utilities
{
    public static class SceneLoader
    {
        public static SceneDataPack PreviousSceneData;

        private static TransitionManager _transitionManager;

        private static TransitionManager TransitionManager
        {
            get
            {
                if (_transitionManager != null) return _transitionManager;
                Initialize();
                return _transitionManager;
            }
        }


        public static void Initialize()
        {
            Debug.Log("Initialize");
            if(TransitionManager.Instance == null)
            {
                var resource = Resources.Load("Utilities/TransitionCanvas");
                Object.Instantiate(resource);
            }
            _transitionManager = TransitionManager.Instance;
        }

        public static IObservable<Unit> OnTransitionFinished
        {
            get { return TransitionManager.OnTransitionAnimationFinished; }
        }

        public static void LoadScene(GameScenes scene, SceneDataPack data = null, GameScenes[] additiveLoadScenes = null, bool autoMove = true)
        {
            if(data == null)
            {
                data = new DefaultSceneDataPack(TransitionManager.CurrentGameScene, additiveLoadScenes);
            }
            TransitionManager.StartTransaction(scene, data, additiveLoadScenes, autoMove);
        }
    }
}