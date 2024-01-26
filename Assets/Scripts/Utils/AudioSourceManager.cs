using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager
{
    private GameObject audioSourceHolder;
    private int audioSourceCount;
    private AudioSource[] audioSources;

    private PriorityQueue<AudioSource> audioSourceQueue = new PriorityQueue<AudioSource>();


    public AudioSourceManager(GameObject audioSourceHolder, int Count)
    {
        this.audioSourceHolder = audioSourceHolder;
        audioSourceCount = Count;
    }



    public void Init()
    {
        audioSources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
        {
            audioSources[i] = audioSourceHolder.AddComponent<AudioSource>();
            audioSources[i].spatialize = false;
            audioSources[i].bypassEffects = true;
            audioSources[i].bypassReverbZones = true;
        }
    }

    public bool GetFreeAudioSource(out AudioSource AS)
    {
        return audioSourceQueue.Dequeue(out AS);
    }

    public void TryReleaseAudioSource()
    {
        for(int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                audioSourceQueue.Enqueue(audioSources[i], i);
            }
        }
    }


}
