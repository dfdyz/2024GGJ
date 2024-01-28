using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public SettingHolderSO settingData;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustAudioOffset();
    }

    public bool beginAdjOff = false;
    int AdjOff_beat = 0;
    bool AdjOff_enable_beat = false;
    double AdjOff_mark_time = 0;
    double AdjOff_marked_time = 0;
    public double[] AdjOff_sample = { 0,0,0,0 };

    void AdjustAudioOffset()
    {
        if (!beginAdjOff) return;
        if (AdjOff_enable_beat && Input.GetKeyDown(KeyCode.Space))
        {
            AdjOff_marked_time = Time.realtimeSinceStartupAsDouble;
        }
    }


    Coroutine adjCor = null;
    public void StartAdjustAudioOffset()
    {
        if (beginAdjOff) return;
        StopAdjust();
        StartCoroutine(IAdjustAudioOffset());
    }

    public void StopAdjust()
    {
        if (adjCor != null)
        {
            StopCoroutine(adjCor);
            adjCor = null;
        }
    }

    IEnumerator IAdjustAudioOffset()
    {
        AdjOff_beat = 0;
        MainMenuButtons.adjustedOff = false;
        beginAdjOff = true;
        AdjOff_enable_beat = false;
        for (int i = 0; i < AdjOff_sample.Length; ++i)
        {
            AdjOff_sample[i] = 0;
        }
        yield return new WaitForSeconds(1f);



        for (int i = 0; i < AdjOff_sample.Length; ++i)
        {
            ++AdjOff_beat;
            AudioManager.Instance.PlayAudio("beat");

            yield return new WaitForSeconds(0.6f);
            ++AdjOff_beat;
            AudioManager.Instance.PlayAudio("beat");
            AdjOff_enable_beat = true;
            yield return new WaitForSeconds(0.6f);
            ++AdjOff_beat;
            AdjOff_mark_time = Time.realtimeSinceStartupAsDouble;
            AudioManager.Instance.PlayAudio("heavybeat");
            yield return new WaitForSeconds(1.2f);
            AdjOff_enable_beat = false;

            AdjOff_sample[i] = AdjOff_marked_time - AdjOff_mark_time;
        }

        double ao = 0;
        for (int i = 0; i < AdjOff_sample.Length; i++)
        {
            ao += AdjOff_sample[i];
        }

        ao /= AdjOff_sample.Length;

        settingData.audioOffset = ao;

        yield return new WaitForSeconds(1f);
        beginAdjOff = false;

        adjCor = null;

        MainMenuButtons.adjustedOff = true;
    }



}
