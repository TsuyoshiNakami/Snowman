using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class SoundInfo {
	[SerializeField] public string soundName;
    [System.NonSerialized] public AudioSource audioSource;
	[SerializeField] public AudioClip clip;
	[SerializeField][Range(0f, 1f)] public float volume = 1;
	[SerializeField] public bool intro = false;
	[SerializeField] public float introLength = 0f;
    [SerializeField] public bool canBeInterrupted = false;
    [SerializeField] public bool loop = false;
}



public class SoundManager : SingletonMonoBehaviourFast<SoundManager> {

	// 楽曲情報
	[SerializeField] SoundInfo[] BGM;
	[SerializeField] bool BGM_ON = true;
	[SerializeField] SoundInfo[] SE;
	[SerializeField] bool SE_ON = true;
	AudioSource Aus, AusIntro, Aus2;
	[Range(0f, 1f)] public float MasterVolume = 1;
	int bgmNumber = 1;
	float initBGMVolume = 0f;
	float initBGMVolume2 = 0f;
	public float fadeOut = 0f;
	public float pitchDownValue = 0.01f;
	float fadeOutConst = 0f;

	public float pitchDown = 0f;
	float pitchDownConst = 0f;
	GameObject SEObj;
	[SerializeField] int samples;
	public GameObject hpGauge;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
		initSE ();
		if (!BGM_ON) {
			return;
		}
		PlayBGM ("Title");
		//Aus2.volume = 0;
	}
	public void Init() {
		if (!BGM_ON) {
			return;
		}
		if (Aus.clip.name != "Stage1" && Aus.clip.name != "PreEnding") {
			PlayBGM ("Stage1Only");
		}
		pitchDown = 0;
		Aus.pitch = 1;
		//Aus2.pitch = 1;
		fadeOut = 0;
		Aus.volume = initBGMVolume;
		//hpGauge.SetActive (true);
		//Aus2.volume = initBGMVolume2;
	}


	public void Play(AudioSource Aus, bool loop) {
		Aus.loop = loop;
		Aus.volume = MasterVolume * Aus.volume;
		Aus.Play();
	}


	public void PlayBGM(string soundName) {
		fadeOut = 0;
		if (!BGM_ON) {
			return;
		}
		Aus = transform.Find("BGM").gameObject.GetComponent<AudioSource>();
		AusIntro = transform.Find("BGMIntro").gameObject.GetComponent<AudioSource>();
		Aus.timeSamples = 0;
		bool isIntro = false;
		float introTime = 0f;
		foreach (SoundInfo si in BGM) {

			if (si.soundName == soundName) {
				Aus.clip = si.clip;
				Aus.volume = si.volume;
				initBGMVolume = Aus.volume;
				if (si.intro) {
					introTime = si.introLength;
					isIntro = true;

					foreach (SoundInfo siIntro in BGM) {
						//	Debug.Log (soundName + "Intro");
						if (siIntro.soundName == soundName + "Intro") {
							AusIntro.clip = siIntro.clip;

							break;
						}
					}
				}
				break;
			}
		}
		if (isIntro) {
			Play (AusIntro, false);
			Invoke ("PlayBGMAfterIntro", introTime);
		} else	if(Aus) {
			Play (Aus, true);
		}
	}
	public void PlayBGM(string soundName, bool isLoop) {
		fadeOut = 0;
		if (!BGM_ON) {
			return;
		}
		Aus = transform.Find("BGM").gameObject.GetComponent<AudioSource>();
		AusIntro = transform.Find("BGMIntro").gameObject.GetComponent<AudioSource>();
		bool isIntro = false;
		float introTime = 0f;
		foreach (SoundInfo si in BGM) {

			if (si.soundName == soundName) {
				Aus.clip = si.clip;
				Aus.volume = si.volume;
				initBGMVolume = Aus.volume;
				if (si.intro) {
					introTime = si.introLength;
					isIntro = true;

					foreach (SoundInfo siIntro in BGM) {
						//	Debug.Log (soundName + "Intro");
						if (siIntro.soundName == soundName + "Intro") {
							AusIntro.clip = siIntro.clip;

							break;
						}
					}
				}
				break;
			}
		}
		if (isIntro) {
			Play (AusIntro, false);
			Invoke ("PlayBGMAfterIntro", introTime);
		} else	if(Aus) {
			Play (Aus, isLoop);
		}
	}


	public void initSE() {
		SEObj = GameObject.Find ("SE");
		foreach(SoundInfo se in SE) {
			AudioSource aus = SEObj.AddComponent<AudioSource> ();
			aus.clip = se.clip;
            se.audioSource = aus;
            aus.loop = se.loop;
		}
	}

	public void PlaySEOneShot(string soundName) {
		if (!SE_ON) {
			return;
		}
		AudioSource[] AusSE = transform.Find("SE").gameObject.GetComponents<AudioSource>();

		foreach (SoundInfo si in SE) {
			if (si.soundName == soundName) {
				for (int i = 0; i < AusSE.Length; i++) {
					if (AusSE [i].clip == si.clip) {
						AusSE[i].volume = si.volume;
						AusSE[i].volume = MasterVolume * AusSE[i].volume;
						AusSE[i].PlayOneShot (AusSE[i].clip);
						break;
					}
				}


			}
		}
	}

    public void PlaySE(string soundName)
    {
        if (!SE_ON)
        {
            return;
        }
        foreach (SoundInfo si in SE)
        {
            if (si.soundName == soundName)
            {
                si.audioSource.volume = si.volume * MasterVolume;
                si.audioSource.Play();
            }
        }
    }

    public void PlaySEIfNotPlaying(string soundName)
    {
        if (!SE_ON)
        {
            return;
        }
        foreach (SoundInfo si in SE)
        {
            if (si.soundName == soundName)
            {
                if(!si.audioSource.isPlaying) {
                    si.audioSource.volume = si.volume * MasterVolume;
                    si.audioSource.Play();
                }
            }
        }
    }

    public void StopSE(string soundName)
    {
        if (!SE_ON)
        {
            return;
        }
        foreach (SoundInfo si in SE)
        {
            if (si.soundName == soundName)
            {
                si.audioSource.Stop();
            }
        }
    }

    public void PitchDown(float time) {
		if (!BGM_ON) {
			return;
		}
		pitchDownConst = Aus.pitch / time / time;
		pitchDown = time;
	}

	public void FadeOut(float time) {
		if (!BGM_ON) {
			return;
		}
		if (Aus.volume != 0) {
			fadeOutConst = Aus.volume / time / time;
		} else {
			fadeOutConst = Aus2.volume / time / time;
		}
		fadeOut = time;
	}

    int stopSample = 0;
    public void StopBGM()
    {
        if(Aus == null)
        {
            return;
        }
        stopSample = Aus.timeSamples;
        Aus.Stop();
    }

    public void PlayBGM()
    {
        if(Aus == null)
        {
            return;
        }
        Aus.Play();
        Aus.timeSamples = stopSample;
    }
	// Update is called once per frame
	void Update () {
		if (!BGM_ON) {
			return;
		}

		if (fadeOut > 0) {
			fadeOut -= Time.deltaTime;
			if (bgmNumber == 1) {
				Aus.volume = fadeOutConst * fadeOut * fadeOut;
			}
			else {
		//		Aus2.volume = fadeOutConst * fadeOut * fadeOut;
			}
		//	Debug.Log (Aus.volume);
		} else if (fadeOut < 0) {
			fadeOut = 0;
			//Aus.Stop ();
		}
		if (pitchDown > 0) {
			pitchDown -= Time.deltaTime;
			Aus.pitch -= pitchDownValue;
		//	Aus2.pitch -= pitchDownValue;
		//	Aus.pitch = pitchDownConst * pitchDown * pitchDown;
			//	Debug.Log (Aus.volume);
		} else if (pitchDown < 0) {
			pitchDown = 0;
			//Aus.Stop ();
			//Aus2.Stop ();
		}
	}
}
