using System;
using System.Collections.Concurrent;
using UnityEngine;

public class TimelineManager
{
    double startedTick = 0;
    // double thisTick = 0;
    double lastBeatTick = 0;
    double lastLogicBeatTick = 0;
    double lastJudgmentBeatTick = 0;

    double bpm = 0;

    double timePerBeat = 0;

    public delegate void BeatEvent();

    public BeatEvent onBaseBeat = () => {}; //ÉùÒôÖá

    public BeatEvent onLogicBaseBeat = () => { };  //Âß¼­Öá
    public BeatEvent onJudgmentEndTick = () => { };  //Ä³ÅÄÅÐ¶¨½áÊøµÄÊÂ¼þ

    public double ms_JudgmentInterval_pos = 0;
    public double ms_JudgmentInterval_neg = 0;

    public double judgmentOffset = 0;
    public TimelineManager() {
    }

    

    double nextBeat = 0;
    double nextLogicBeat = 0;
    double nextJudgmentEnd = 0;
    public void Tick()
    {

        //base beat(audio)
        nextBeat = GetNextBaseBeatTick();
        if (Time.realtimeSinceStartupAsDouble >= nextBeat)
        {
            lastBeatTick = nextBeat;
            onBaseBeat();
        }

        // logic beat
        nextLogicBeat = GetNextLogicBaseBeatTick();
        if (Time.realtimeSinceStartupAsDouble >= nextLogicBeat)
        {
            lastLogicBeatTick = nextLogicBeat;
            onLogicBaseBeat();
        }

        // judgment end
        nextJudgmentEnd = GetNextJudgmentBeatTick();
        if (Time.realtimeSinceStartupAsDouble >= nextJudgmentEnd)
        {
            lastLogicBeatTick = nextJudgmentEnd;
            onJudgmentEndTick();
        }

        //UnityEngine.Debug.Log("next beat: " + nextBeat);
        //UnityEngine.Debug.Log("time: " + Time.realtimeSinceStartupAsDouble);
    }

    public bool JudgementNextBeat()
    {
        return Time.realtimeSinceStartupAsDouble >= GetNextLogicBaseBeatTick() - ms_JudgmentInterval_neg;
    }

    public bool JudementThisBeat()
    {
        return Time.realtimeSinceStartupAsDouble <= lastLogicBeatTick + ms_JudgmentInterval_pos;
    }

    public void Start(int bpm)
    {
        SetBpm(bpm);
        Start();
    }


    public void Start()
    {
        startedTick = Time.realtimeSinceStartupAsDouble;
        lastBeatTick = startedTick;
        lastLogicBeatTick = lastBeatTick + judgmentOffset;
        lastJudgmentBeatTick = lastLogicBeatTick + ms_JudgmentInterval_pos;
    }

    public double GetNextBaseBeatTick()
    {
        return lastBeatTick + timePerBeat;
    }

    public double GetNextLogicBaseBeatTick()
    {
        return lastLogicBeatTick + timePerBeat;
    }

    public double GetNextJudgmentBeatTick()
    {
        return lastJudgmentBeatTick + timePerBeat;
    }

    public void SetBpm(double bpm)
    {
        this.bpm = bpm;
        timePerBeat = 60 / bpm;
    }

}