using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;
using static SkillActionSheetSO;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        timeline = new TimelineManager(() => audioOffset);
    }

    [Header("Resources")]
    public SkillActionSheetSO skillActionSheet;
    public SettingHolderSO settingData;
    public PlayDataHolderSO gameData;

    public SkillClip[] playerSkillRegistries;
    public SkillClip[] mobSkillRegistries;

    [Header("Parameters")]
    public int bpm = 120;

    public int ms_JudgmentInterval_pos = 80; // 正向
    public int ms_JudgmentInterval_neg = 80; // 反向
    public int ms_JudgmentInterval_add = 70; // 双click补偿

    [Header("Display")]
    public int lastSucessBeat = 0;


    public int currentBeat { get; private set; } //逻辑拍
    private int currentAudioBeat = 0;

    public TimelineManager.BeatEvent onHeavyBeat = () => { };
    public TimelineManager.BeatEvent onNormalBeat = () => { };
    public TimelineManager.BeatEvent onBeat = () => { };

    public TimelineManager.BeatEvent onGameStart = () => { };

    public double audioOffset
    {
        get => settingData.audioOffset;
        set => settingData.audioOffset = value;
    }

    public bool isStarted { get; private set; }
    public TimelineManager timeline {  get; private set; }

    public void BattleStart()
    {
        if (!isStarted) {
            gameData.ResetData();
            isStarted = true;

            currentBeat = 0;
            currentAudioBeat = 0;
            lastSucessBeat = 0;

            timeline.s_JudgmentInterval_pos = ms_JudgmentInterval_pos / 1000.0;
            timeline.s_JudgmentInterval_neg = ms_JudgmentInterval_neg / 1000.0;
            timeline.s_JudgmentInterval_additional = ms_JudgmentInterval_add / 1000.0;
            timeline.Start(bpm);

            onGameStart();
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

    public void HurtPlayer(int dmg)
    {
        gameData.playerHealth -= dmg;
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
            timeline.Tick(lastSucessBeat >= currentBeat);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeline.onBaseBeat = onBaseBeat;
        timeline.onLogicBaseBeat = onLogicBaseBeat;

        playerSkillRegistries = skillActionSheet.BakePlayerSkill();
        mobSkillRegistries = skillActionSheet.mobSkillRegistries;
    }

    public void AddScore(int score)
    {
        gameData.Score += score;
    }

    void onBaseBeat()
    {

        ++currentAudioBeat;
        //onBeat();
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
        //print("bb");
        ++currentBeat;
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
        onBeat();
    }

    public int Judgment()
    {
        if (GetModifiedCurrBeat() >= currentBeat && timeline.JudgmentThisBeatAdditional())
        {
            return currentBeat;
        }
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
