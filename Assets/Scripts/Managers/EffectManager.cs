using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager instance;
    // Start is called before the first frame update
    [Serializable]
    public struct EffectRegistriy
    {
        public string name;
        public EffectBase effect;
    }

    [SerializeField] EffectRegistriy[] registries;
    private Dictionary<string, int> registriesMap = new Dictionary<string, int>();

    public void RegisterAll()
    {
        for (int i = 0; i < registries.Length; i++)
        {
            registriesMap.Add(registries[i].name, i);
        }
    }

    public static EffectBase GetEffectInctanceByName(string name)
    {
        return instance.registries[instance.registriesMap.GetValueOrDefault(name, 0)].effect;
    }

    private void Awake()
    {
        instance = this;
        RegisterAll();
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
