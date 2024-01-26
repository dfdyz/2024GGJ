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



    public int bpm = 120;

    public int ms_JudgmentInterval = 80;

    [SerializeField]
    public int currentBeat {  get; private set; }

    public int lastSucessBeat = 0;


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
            lastSucessBeat = 0;

            timeline.judgmentInterval = ms_JudgmentInterval / 1000.0;
            timeline.Start(bpm);
        }
        else
        {
            BattleEnd();
            BattleStart();
        }
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
        audioOffset = 0.226;
    }

    void onBaseBeat()
    {
        if(currentBeat % 4 == 0)
        {
            AudioManager.Instance.PlayAudio("heavybeat");
        }
        else
        {
            AudioManager.Instance.PlayAudio("beat");
        }
        ++currentBeat;

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
