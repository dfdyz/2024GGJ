using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Resources")]

    [Header("Parameters")]
    [SerializeField] int MaxHealth = 100;

    [SerializeField] float moveSmooth = 0.25f;


    [Header("Display")]
    public int Health;

    private PlayerInputBuffer inputCache = new PlayerInputBuffer(8);


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onBeat = onBeat;
        GameManager.Instance.onHeavyBeat = onHeavyBeat;

        DebugManager.Instance.debugWindowAttachmentFunc += DebugInfo;
    }

    void DebugInfo()
    {
        GUILayout.Label(inputCache.ToString());
    }

    public void ResetPlayer()
    {
        Health = MaxHealth;
    }


    // Update is called once per frame

    int judgmentBeat = 0;
    void Update()
    {
        if (!GameManager.Instance.isStarted) return;

        if (Input.anyKeyDown)
        {
            judgmentBeat = GameManager.Instance.Judgment();
            if (judgmentBeat > 0)
            {
                if (judgmentBeat != GameManager.Instance.lastSucessBeat) // first ctrl
                {
                    if (HandleJudgmentFirst())
                    {
                        GameManager.Instance.lastSucessBeat = judgmentBeat;
                        print("Beated");
                    }
                }
                else
                {
                    HandleJudgmentSecond();
                }
            }
            else
            {
                print("Miss");
            }
        }

        MovementVisual();
    }



    int slot;
    bool HandleJudgmentFirst()
    {
        //print(judgmentBeat + " " + GameManager.Instance.currentBeat);
        slot = (judgmentBeat - 1) % 4;
        if (slot == 0) //clear input cache
        {
            inputCache.Clear();
        }

        bool flag = false;

        if (Input.GetKeyDown(KeyCode.A)) //Move L
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveL);

            GridManager.Instance.playerAt -= 1;

            flag = true;
        }
        if (Input.GetKeyDown(KeyCode.D)) //Move R
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveR);

            GridManager.Instance.playerAt += 1;

            flag = true;
        }
        if(Input.GetKeyDown(KeyCode.J)) //Atk`                             
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.Accept);

            flag = true;
        }

        return flag;
    }

    


    void HandleJudgmentSecond()
    {
        slot = (judgmentBeat - 1) % 4;
        if (Input.GetKeyDown(KeyCode.A)) //Move L
        {
            if (inputCache.GetBufferedCount(slot) == 1 && inputCache.GetBufferedType(slot, 0) == PlayerInputBuffer.InputType.Accept)
            {
                GridManager.Instance.playerAt -= 1;
            }


            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveL);
        }
        if (Input.GetKeyDown(KeyCode.D)) //Move R
        {
            if (inputCache.GetBufferedCount(slot) == 1 && inputCache.GetBufferedType(slot, 0) == PlayerInputBuffer.InputType.Accept)
            {
                GridManager.Instance.playerAt += 1;
            }

            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveR);
        }
        if (Input.GetKeyDown(KeyCode.J)) //Atk`                             
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.Accept);
        }
    }







    void onBeat()
    {
        if(GameManager.Instance.currentBeat % 4 == 3)// try clear player input buffer
        {

        }
    }

    void onHeavyBeat()
    {
        if(GameManager.Instance.lastSucessBeat == GameManager.Instance.currentBeat)
        {

        }
        else
        {
            inputCache.Clear();
        }
    }






    private void FixedUpdate()
    {
        //if (!GameManager.Instance.isStarted) return;
        
    }


    void MovementVisual()
    {
        Vector2 targetPos = GridManager.Instance.GetPlayerVisualPos();
        this.gameObject.transform.position = Vector2.Lerp(this.gameObject.transform.position, targetPos, moveSmooth);
    }

}
