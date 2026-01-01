using System;
using System.Collections.Generic;
using UnityEngine;

public class UT_AudioRoot : MonoBehaviour
{
    [SerializeField] private AudioSource _MusicSource;
    public AudioSource MusicSource => _MusicSource;

    [SerializeField] private AudioSource _FXSource;
    public AudioSource FXSource => _FXSource;

    private void Awake()
    {
        gameObject.name = "AudioRoot";
        DontDestroyOnLoad(this);
    }
}
