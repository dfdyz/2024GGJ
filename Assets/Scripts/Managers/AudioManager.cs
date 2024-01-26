using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField]
    private GameObject audioSourceHolder;

    private AudioSourceManager audioSourceManager;

    [Serializable]
    struct Registry
    {
        public string name;
        public AudioClip audio;
    }

    [SerializeField]
    private Registry[] registries_;

    private Dictionary<string, AudioClip> registries;

    private void Awake()
    {
        Instance = this;
        registries = new Dictionary<string, AudioClip>();

        for(int i = 0; i < registries_.Length; i++)
        {
            try
            {
                registries.Add(registries_[i].name, registries_[i].audio);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

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
