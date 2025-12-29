using System;
using System.Collections.Generic;
using UnityEngine;

public class UT_AudioRoot : MonoBehaviour
{
    [SerializeField] public AudioSource _MusicSource;
    [SerializeField] public AudioSource _FXSource;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
