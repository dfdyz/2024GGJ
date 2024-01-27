using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Header("Resources")]
    public SkillActionSheetSO skillActionSheets;



    [Header("Parameters")]
    public int bpm = 120;

    public int ms_JudgmentInterval_pos = 80; // 正向
    public int ms_JudgmentInterval_neg = 80; // 反向


    [Header("Display")]
    public int lastSucessBeat = 0;


    public int currentBeat { get; private set; } //逻辑拍
    private int currentAudioBeat = 0;

    public TimelineManager.BeatEvent onHeavyBeat = () => { };
    public TimelineManager.BeatEvent onNormalBeat = () => { };
    public TimelineManager.BeatEvent onBeat = () => { };

    public double audioOffset
    {
        get => timeline.judgmentOffset;
        set => timeline.judgmentOffset = value;
    }

    public bool isStarted { get; private set; }
    public TimelineManager timeline {  get; private set; } = new TimelineManager();

    public void BattleStart()
    {
        if (!isStarted) {
            isStarted = true;

            currentBeat = 0;
            currentAudioBeat = 0;
            lastSucessBeat = 0;

            timeline.ms_JudgmentInterval_pos = ms_JudgmentInterval_pos / 1000.0;
            timeline.ms_JudgmentInterval_neg = ms_JudgmentInterval_neg / 1000.0;
            timeline.Start(bpm);
        }
        else
        {
            BattleEnd();
            BattleStart();
        }
    }

    public int GetModifiedCurrBeat()
    {
        return Math.Max(currentBeat, lastSucessBeat);
    }


    public void BattleEnd()
    {
        if(isStarted)
        {
            isStarted = false;
        }
    }

    private void FixedUpdate()
    {
        if (isStarted)
        {
            timeline.Tick();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeline.onBaseBeat = onBaseBeat;
        timeline.onLogicBaseBeat = onLogicBaseBeat;
        audioOffset = 0.18;
    }




    void onBaseBeat()
    {
        ++currentAudioBeat;
        onBeat();
        if((currentAudioBeat - 1) % 4 == 0)
        {
            AudioManager.Instance.PlayAudio("heavybeat");
            //onHeavyBeat();
        }
        else
        {
            AudioManager.Instance.PlayAudio("beat");
            //onNormalBeat();
        }
    }

    void onLogicBaseBeat()
    {
        ++currentBeat;
        onBeat();
        if ((currentBeat - 1) % 4 == 0)
        {
            //AudioManager.Instance.PlayAudio("heavybeat");
            onHeavyBeat();
        }
        else
        {
            //AudioManager.Instance.PlayAudio("beat");
            onNormalBeat();
        }
    }

    public int Judgment()
    {
        if (timeline.JudementThisBeat())
        {
            return currentBeat;
        }
        if (timeline.JudgementNextBeat())
        {
            return currentBeat + 1;
        }

        return -1;
    }


    
   
   
 


    // Update is called once per frame
    void Update()
    {
        
    }

   
}
