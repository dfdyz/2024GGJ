using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
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

    bool beginAdjOff = false;
    int AdjOff_beat = 0;
    bool AdjOff_enable_beat = false;
    double AdjOff_mark_time = 0;
    double AdjOff_marked_time = 0;
    double[] AdjOff_sample = { 1,1,1,1 };

    void AdjustAudioOffset()
    {
        if (!beginAdjOff) return;
        if (AdjOff_enable_beat && Input.GetKeyDown(KeyCode.Space))
        {
            AdjOff_marked_time = Time.realtimeSinceStartupAsDouble;
        }
    }

    public void StartAdjustAudioOffset()
    {
        StartCoroutine(IAdjustAudioOffset());
    }

    IEnumerator IAdjustAudioOffset()
    {
        AdjOff_beat = 0;
        beginAdjOff = true;
        AdjOff_enable_beat = false;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < AdjOff_sample.Length; i++)
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

        GameManager.Instance.audioOffset = ao;
    }



}
