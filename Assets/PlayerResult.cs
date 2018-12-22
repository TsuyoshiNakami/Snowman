using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResult : MonoBehaviour {

    [SerializeField] GameObject[] presents;
    public void ThrowPresent()
    {
        int i = Random.Range(0, 2);
        Instantiate(presents[i], transform.position, transform.rotation);
    }

}
