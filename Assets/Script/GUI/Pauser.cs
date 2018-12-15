using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if  UNITY_EDITOR
using UnityEditor;
#endif

public enum PauseType
{
    Freeze,
    KeepRigidbody,
}

public enum PauseMode
{
    Normal,
    Force
}

public class Pauser : MonoBehaviour
{
    public static List<Pauser> targets = new List<Pauser>();    // ポーズ対象のスクリプト
    public static bool isPausing = false;
    // ポーズ対象のコンポーネント
    Behaviour[] pauseBehavs = null;

    Rigidbody[] rgBodies = null;
    Vector3[] rgBodyVels = null;
    Vector3[] rgBodyAVels = null;

    Rigidbody2D[] rg2dBodies = null;
    Vector2[] rg2dBodyVels = null;
    float[] rg2dBodyAVels = null;

    [SerializeField] PauseType pauseType;


    // 初期化
    void Start()
    {

        // ポーズ対象に追加する
        targets.Add(this);
    }

    // 破棄されるとき
    void OnDestory()
    {

        // ポーズ対象から除外する
        targets.Remove(this);
    }
    
    // ポーズされたとき
    void OnPause(PauseMode mode)
    {

        if (pauseBehavs != null)
        {
            return;
        }

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (targets[i] == null)
            {
                targets.Remove(targets[i]);
            }
        }
        if (mode == PauseMode.Normal)
        {
            if (pauseType == PauseType.KeepRigidbody)
            {
                return;
            }
        }
        // 有効なコンポーネントを取得

            pauseBehavs = Array.FindAll(GetComponentsInChildren<Behaviour>(), (obj) => { return obj.enabled; });
            foreach (var com in pauseBehavs)
            {
                com.enabled = false;
            }

            rgBodies = Array.FindAll(GetComponentsInChildren<Rigidbody>(), (obj) => { return !obj.IsSleeping(); });
            rgBodyVels = new Vector3[rgBodies.Length];
            rgBodyAVels = new Vector3[rgBodies.Length];
            for (var i = 0; i < rgBodies.Length; ++i)
            {
                rgBodyVels[i] = rgBodies[i].velocity;
                rgBodyAVels[i] = rgBodies[i].angularVelocity;
                rgBodies[i].Sleep();
            }

            rg2dBodies = Array.FindAll(GetComponentsInChildren<Rigidbody2D>(), (obj) => { return !obj.IsSleeping(); });
            rg2dBodyVels = new Vector2[rg2dBodies.Length];
            rg2dBodyAVels = new float[rg2dBodies.Length];

            for (var i = 0; i < rg2dBodies.Length; ++i)
            {
                rg2dBodyVels[i] = rg2dBodies[i].velocity;
                rg2dBodyAVels[i] = rg2dBodies[i].angularVelocity;
                rg2dBodies[i].Sleep();
            }

    }

    // ポーズ解除されたとき
    void OnResume()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // ポーズ前の状態にコンポーネントの有効状態を復元
        foreach (var com in pauseBehavs)
        {
            if (com != null)
            {
                com.enabled = true;
            }
        }

        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodies[i].WakeUp();
            rgBodies[i].velocity = rgBodyVels[i];
            rgBodies[i].angularVelocity = rgBodyAVels[i];
        }

        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
            if (rg2dBodies[i] == null)
            {
                continue;
            }
            rg2dBodies[i].WakeUp();
            rg2dBodies[i].velocity = rg2dBodyVels[i];
            rg2dBodies[i].angularVelocity = rg2dBodyAVels[i];
        }

        pauseBehavs = null;

        rgBodies = null;
        rgBodyVels = null;
        rgBodyAVels = null;

        rg2dBodies = null;
        rg2dBodyVels = null;
        rg2dBodyAVels = null;
    }
    // ポーズ
    public static void Pause(PauseMode mode = PauseMode.Normal)
    {
        targets.RemoveAll(item => item == null);
        isPausing = true;

        foreach (var obj in targets)
        {
            obj.OnPause(mode);
        }
    }

    public static void PauseWithoutTag(string tag, PauseMode mode = PauseMode.Normal)
    {
        targets.RemoveAll(item => item == null);
        isPausing = true;

        foreach (var obj in targets)
        {
            if (obj.tag != tag)
            {
                obj.OnPause(mode);
            }
        }
    }
    // ポーズ
    public static void PauseWithout(string name)
    {
        isPausing = true;
        foreach (var obj in targets)
        {

            if (obj.name != name)
                obj.OnPause(PauseMode.Normal);

        }
    }
    public static void Pause(string name)
    {
        isPausing = true;
        foreach (var obj in targets)
        {

            if (obj.name == name)
                obj.OnPause(PauseMode.Normal);
            break;
        }
    }

    public static void Pause(string name, PauseMode mode)
    {
        isPausing = true;
        foreach (var obj in targets)
                {

                    if (obj.name == name)
                        obj.OnPause(mode);
                    break;
                }


    }


    public static void Resume(string name)
    {

        foreach (var obj in targets)
        {

            if (obj.name == name)
            {
                obj.OnResume();

            }

        }
    }
    // ポーズ解除
    public static void Resume()
    {

        isPausing = false;
        foreach (var obj in targets)
        {

            obj.OnResume();
        }

    }
}
