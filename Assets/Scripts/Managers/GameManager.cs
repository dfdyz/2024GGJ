using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
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
    public bool realStarted = false;

    private int currentAudioBeat = 0;

    public TimelineManager.BeatEvent onHeavyBeat = () => { };
    public TimelineManager.BeatEvent onNormalBeat = () => { };
    public TimelineManager.BeatEvent onBeat = () => { };
    public TimelineManager.BeatEvent onJudgmentEndTick = () => { };

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
            realStarted = false;

            currentBeat = 0;
            currentAudioBeat = 0;
            lastSucessBeat = 0;

            timeline.s_JudgmentInterval_pos = ms_JudgmentInterval_pos / 1000.0;
            timeline.s_JudgmentInterval_neg = ms_JudgmentInterval_neg / 1000.0;
            timeline.s_JudgmentInterval_additional = ms_JudgmentInterval_add / 1000.0;
            timeline.Start(bpm);

            UIManager.Instance.SetBeaterPos(0);

            GridManager.Instance.InitBattlePos();
            onGameStart();
        }
        else
        {
            BattleEnd();
            BattleStart();
        }
    }

    void JudgmentEnd()
    {
        if(realStarted)
        {
            onJudgmentEndTick();
        }
        else
        {
            if(currentBeat >= 4)
            {
                realStarted = true;
                currentBeat -= 4;
                lastSucessBeat = 0;
            }
        }
    }


    void SettlementGame()
    {
        BattleEnd();
        StartCoroutine(SettlementGame_());
    }

    IEnumerator SettlementGame_()
    {
        yield return new WaitForSeconds(1);
        CGManager.PlayCG(settingData, "BeforeSettlementGame", "ScoreScene");
    }

    public int GetModifiedCurrBeat()
    {
        return Math.Max(currentBeat, lastSucessBeat);
    }

    public void HurtPlayer(int dmg)
    {
        gameData.playerHealth -= dmg;

        if(gameData.playerHealth <= 0)
        {
            SettlementGame();
        }
    }

    public void BattleEnd()
    {
        if(isStarted)
        {
            isStarted = false;
            realStarted = false;
        }
    }

    private void FixedUpdate()
    {
        if (isStarted)
        {
            timeline.Tick(lastSucessBeat >= currentBeat);
        }
    }

    public bool suspend { get => timeline.suspend; }

    public void Suspend()
    {
        timeline.Suspend();
    }

    public void Resume()
    {
        timeline.Resume();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        timeline.onBaseBeat = onBaseBeat;
        timeline.onLogicBaseBeat = onLogicBaseBeat;
        timeline.onJudgmentEndTick = JudgmentEnd;

        playerSkillRegistries = skillActionSheet.BakePlayerSkill();
        mobSkillRegistries = skillActionSheet.mobSkillRegistries;

        //GridManager.Instance.InitBattlePos();

        CameraFallow.ResetCamera();

        if (!DebugManager.Instance.debugMode) {
            StartCoroutine(PlayModeStart());
        }
    }

    IEnumerator PlayModeStart()
    {
        yield return new WaitForSeconds(1);
        BattleStart();
    }

    public void AddScore(int score)
    {
        gameData.Score += score;
    }

    void onBaseBeat()
    {
        ++currentAudioBeat;

        if (currentAudioBeat - 4 > settingData.subsectionCount * 4) return;

        //onBeat();
        if(currentAudioBeat <= 4)
        {
            AudioManager.Instance.PlayAudio("prevbeat");
        }
        else
        {
            if ((currentAudioBeat - 1) % 4 == 0)
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
    }

    
    void onLogicBaseBeat()
    {
        //print("bb");
        ++currentBeat;
        if (realStarted)
        {
            if (currentBeat > settingData.subsectionCount * 4)
            {
                SettlementGame();
            }

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
        else
        {
            
            if(currentBeat >= 5)
            {
                realStarted = true;
                currentBeat -= 4;
                lastSucessBeat = 0;
            }
        }

        UIManager.Instance.SetBeaterPos((currentBeat - 1) % 4);

    }


    public int Judgment()
    {
        if (!realStarted) return -1;
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
