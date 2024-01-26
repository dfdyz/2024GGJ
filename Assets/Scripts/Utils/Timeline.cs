using System;
using System.Collections.Concurrent;
using UnityEngine;

public class TimelineManager
{
    double startedTick = 0;
    // double thisTick = 0;
    double lastBeatTick = 0;

    double bpm = 0;

    double timePerBeat = 0;

    public delegate void BeatEvent();

    public BeatEvent onBaseBeat = () => {};

    public double ms_JudgmentInterval_pos = 0;
    public double ms_JudgmentInterval_neg = 0;

    public double judgmentOffset = 0;
    public TimelineManager() {
    }

    

    double nextBeat = 0;
    public void Tick()
    {
        //base beat
        nextBeat = GetNextBaseBeatTick();
        if (Time.realtimeSinceStartupAsDouble >= nextBeat)
        {
            lastBeatTick = nextBeat;
            onBaseBeat();
        }
        //UnityEngine.Debug.Log("next beat: " + nextBeat);
        //UnityEngine.Debug.Log("time: " + Time.realtimeSinceStartupAsDouble);
    }

    public bool JudgementNextBeat()
    {
        return Time.realtimeSinceStartupAsDouble >= GetNextBaseBeatTick() - ms_JudgmentInterval_neg + judgmentOffset;
    }

    public bool JudementThisBeat()
    {
        return Time.realtimeSinceStartupAsDouble <= lastBeatTick + ms_JudgmentInterval_pos + judgmentOffset;
    }

    public void Start(int bpm)
    {
        SetBpm(bpm);
        Start();
    }


    public void Start()
    {
        startedTick = Time.realtimeSinceStartupAsDouble;
        lastBeatTick = Time.realtimeSinceStartupAsDouble;
    }

    public double GetNextBaseBeatTick()
    {
        return lastBeatTick + timePerBeat;
    }



    public void SetBpm(double bpm)
    {
        this.bpm = bpm;
        timePerBeat = 60 / bpm;
    }

}