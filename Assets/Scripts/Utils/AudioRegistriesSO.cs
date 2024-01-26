

using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioRegistriesSO", menuName = "ScriptableObject/AudioRegistriesSO", order = 2)]
public class AudioRegistriesSO : ScriptableObject
{
    [Serializable]
    public struct AudioRegistry
    {
        public string name;
        public AudioClip audio;
    }

    [SerializeField]
    private AudioRegistry[] registries_;

    public Dictionary<string, AudioClip> registries { get; private set; }

    public void Init()
    {
        if(registries == null)
        {
            registries = new Dictionary<string, AudioClip>();
            for (int i = 0; i < registries_.Length; i++)
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
        }
    }

    public bool TryGetValue(string name, out AudioClip audio)
    {
        return registries.TryGetValue(name, out audio);
    }

}