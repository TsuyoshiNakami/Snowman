using UnityEngine;
using Zenject;
using Tsuyomi.Yukihuru.Scripts.Utilities;

public class PauseWindow : MonoBehaviour {
    [SerializeField] GameObject pauseWindowObj;
    [Inject]
    PresentGameManager presentGameManager;
    GameManager gameManager;
    SoundManager soundManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pauseWindowObj.SetActive(false);
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}
	
    public void OnHomeButtonPressed()
    {
        if (pauseWindowObj.activeInHierarchy)
            {
                Pauser.Resume();
                soundManager.PlayBGM();
                pauseWindowObj.SetActive(false);
                soundManager.PlaySEOneShot("Cancel");
            } else
            {
                if (!Pauser.isPausing)
                {
                    Pauser.Pause(PauseMode.Force);
                    soundManager.StopBGM();
                    pauseWindowObj.SetActive(true);
                soundManager.PlaySEOneShot("Decide");
                }
            }
    }

    public void OnDecideButtonPressed()
    {
            if (pauseWindowObj.activeInHierarchy)
            {
                soundManager.PlaySEOneShot("Decide");
                SceneLoader.LoadScene(GameScenes.Title);
            }
    }

    public void OnFireButtonPressed()
    {
    if (pauseWindowObj.activeInHierarchy)
            {
                soundManager.PlaySEOneShot("Decide");
                SceneLoader.LoadScene(GameScenes.Title);
            }

    }
}
