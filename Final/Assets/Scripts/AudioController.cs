using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");
        if (objs.Length > 1)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
