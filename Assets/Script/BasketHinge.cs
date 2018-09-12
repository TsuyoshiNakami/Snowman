using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketHinge : MonoBehaviour {

    float z = 0;
    float vz = 0;
    public bool isClosed = false;

    [SerializeField]float openCloseSpeed = 8;
    float nowOpenCloseSpeed = 8;
    PlatformEffector2D lidPlatform;
    Coroutine openCoroutine;
    Coroutine closeCoroutine;

    GameObject lid;

    // Use this for initialization
    void Start () {
        lid = transform.Find("Lid").gameObject;
        lidPlatform = lid.GetComponent<PlatformEffector2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(z < 5)
        {
            isClosed = true;
        } else
        {
            isClosed = false;
        }
        z += vz;
        transform.rotation = Quaternion.Euler(0, 0, z);
        lidPlatform.rotationalOffset = -z;
    }

    public void BeginOpen()
    {
        if(closeCoroutine != null)
        {
            StopCoroutine(closeCoroutine);
        }
        nowOpenCloseSpeed = openCloseSpeed;

        openCoroutine = StartCoroutine(IOpen());

    }

    public void BeginClose()
    {
        if (openCoroutine != null)
        {
            StopCoroutine(openCoroutine);
        }

        nowOpenCloseSpeed = openCloseSpeed;
        closeCoroutine = StartCoroutine(IClose());

    }

    public void Open()
    {
        z = 180;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }

    public void Close()
    {
        z = 0;
        transform.rotation = Quaternion.Euler(0, 0, z);
    }
    IEnumerator IOpen()
    {
        vz = 0;
        while (true)
        {

                vz += nowOpenCloseSpeed * Time.deltaTime;
            if (transform.rotation.eulerAngles.z > 180)
            {
                z = 180;
                vz *= -1;
                vz *= 0.3f;
                nowOpenCloseSpeed *= 0.6f;
                if (Mathf.Abs(vz) <= 0.5f) break;
            }

            yield return new WaitForFixedUpdate();

        }
        vz = 0;
        yield return null;
    }

    IEnumerator IClose()
    {
        vz = 0;
        while (true)
        {

            vz -= nowOpenCloseSpeed * Time.deltaTime;
            if (transform.rotation.eulerAngles.z  <= 360 &&
                transform.rotation.eulerAngles.z > 270)
            {
                z = 0;
                vz *= -1;
                vz *= 0.3f;
                nowOpenCloseSpeed *= 0.6f;
                if (Mathf.Abs(vz) <= 0.5f) break;
            }

            yield return new WaitForFixedUpdate();

        }
        vz = 0;
        yield return null;
    }
}
