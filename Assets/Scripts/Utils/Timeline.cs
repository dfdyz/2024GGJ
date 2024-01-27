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
    public delegate double DoubleProvider();

    public BeatEvent onBaseBeat = () => {}; //ÉùÒôÖá

    public BeatEvent onLogicBaseBeat = () => { };  //Âß¼­Öá
    public BeatEvent onJudgmentEndTick = () => { };  //Ä³ÅÄÅÐ¶¨½áÊøµÄÊÂ¼þ

    public double s_JudgmentInterval_pos = 0;
    public double s_JudgmentInterval_neg = 0;
    public double s_JudgmentInterval_additional = 0;

    public double judgmentOffset {
        get => audioOffsetGetter();
    }

    private DoubleProvider audioOffsetGetter;

    public TimelineManager(DoubleProvider audioOffsetGetter) {
        this.audioOffsetGetter = audioOffsetGetter;
    }

    double nextBeat = 0;
    double nextLogicBeat = 0;
    double nextJudgmentEnd = 0;
    public void Tick(bool additionalJudgment)
    {

        //base beat(audio)
        double thisTick = Time.realtimeSinceStartupAsDouble;
        nextBeat = GetNextBaseBeatTick();
        if (thisTick >= nextBeat)
        {
            lastBeatTick = nextBeat;
            onBaseBeat();
        }

        // logic beat
        nextLogicBeat = GetNextLogicBaseBeatTick();
        if (thisTick >= nextLogicBeat)
        {
            lastLogicBeatTick = nextLogicBeat;
            onLogicBaseBeat();
        }

        // judgment end
        nextJudgmentEnd = GetNextJudgmentBeatTick(additionalJudgment);
        if (thisTick >= nextJudgmentEnd)
        {
            lastJudgmentBeatTick = nextJudgmentEnd;
            onJudgmentEndTick();
        }


        //UnityEngine.Debug.Log("next beat: " + nextBeat);
        //UnityEngine.Debug.Log("time: " + Time.realtimeSinceStartupAsDouble);
    }

    public bool JudgementNextBeat()
    {
        return Time.realtimeSinceStartupAsDouble >= GetNextLogicBaseBeatTick() - s_JudgmentInterval_neg;
    }

    public bool JudementThisBeat()
    {
        return 
            Time.realtimeSinceStartupAsDouble <= lastLogicBeatTick + s_JudgmentInterval_pos;
    }

    public bool JudgmentThisBeatAdditional()
    {
        return
            Time.realtimeSinceStartupAsDouble <= lastLogicBeatTick + s_JudgmentInterval_pos + s_JudgmentInterval_additional;
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
        lastJudgmentBeatTick = lastLogicBeatTick + s_JudgmentInterval_pos;
    }

    public double GetNextBaseBeatTick()
    {
        return lastBeatTick + timePerBeat;
    }

    public double GetNextLogicBaseBeatTick()
    {
        return lastLogicBeatTick + timePerBeat;
    }

    public double GetNextJudgmentBeatTick(bool additionalJudgment)
    {
        return lastJudgmentBeatTick + timePerBeat + (additionalJudgment ? s_JudgmentInterval_additional:0);
    }

    public void SetBpm(double bpm)
    {
        this.bpm = bpm;
        timePerBeat = 60 / bpm;
    }

}