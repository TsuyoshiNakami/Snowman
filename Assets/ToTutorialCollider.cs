using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tsuyomi.Yukihuru.Scripts.Utilities;

public class ToTutorialCollider : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            SceneLoader.LoadScene(GameScenes.Tutorial);
        }
    }
}
