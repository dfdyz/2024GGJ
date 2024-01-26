using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    [SerializeField]
    private AudioRegistriesSO registries;
    [SerializeField]
    private GameObject audioSourceHolder;

    private AudioSourceManager audioSourceManager;

    private void Awake()
    {
        Instance = this;

        registries.Init();

        audioSourceManager = new AudioSourceManager(audioSourceHolder, 16);
        audioSourceManager.Init();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayAudio(string name)
    {
        AudioClip audio = null;
        registries.TryGetValue(name, out audio);
        if (audio != null)
        {
            AudioSource AS = null;
            if(audioSourceManager.GetFreeAudioSource(out AS)) {
                AS.PlayOneShot(audio);
            }
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource AS = null;
            if (audioSourceManager.GetFreeAudioSource(out AS))
            {
                AS.PlayOneShot(clip);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    private void LateUpdate()
    {
        audioSourceManager.TryReleaseAudioSource();
    }
}
