using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static SkillActionSheetSO;

public class PlayerController : MonoBehaviour
{
    [Header("Resources")]

    [Header("Parameters")]
    [SerializeField] int MaxHealth = 100;

    [SerializeField] float moveSmooth = 0.25f;



    [Header("Display")]
    public HashSet<int> matchedSkill = new HashSet<int>();
    public int Health;
    private PlayerInputBuffer inputCache = new PlayerInputBuffer(8);


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onBeat = onBeat;
        GameManager.Instance.onHeavyBeat = onHeavyBeat;
        GameManager.Instance.timeline.onJudgmentEndTick = onJudgmentEnd;

        DebugManager.Instance.debugWindowAttachmentFunc += DebugInfo;
    }

    void DebugInfo()
    {
        GUILayout.Label(inputCache.ToString());

        string str = "\n";
        foreach (int i in matchedSkill)
        {
            str += i.ToString() + " ";
        }
        GUILayout.Label(str);
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
                else     //first
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

    void MatchAllSkill()
    {
        matchedSkill.Clear();
        for (int i = 0; i < GameManager.Instance.playerkillRegistries.Length; i++)
        {
            matchedSkill.Add(i);
        }
    }

    int slot;
    bool HandleJudgmentFirst()
    {
        //print(judgmentBeat + " " + GameManager.Instance.currentBeat);
        slot = (judgmentBeat - 1) % 4;
        if (slot == 0) //clear input cache
        {
            inputCache.Clear();
            MatchAllSkill();
        }

        bool flag = false;

        if (Input.GetKeyDown(KeyCode.A)) //Move L
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveL);

            GridManager.Instance.playerAt -= 1;

            MatchAndHandleSkill();

            flag = true;
        }
        if (Input.GetKeyDown(KeyCode.D)) //Move R
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveR);

            GridManager.Instance.playerAt += 1;

            MatchAndHandleSkill();

            flag = true;
        }

        if(Input.GetKeyDown(KeyCode.J)) //Atk`                             
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.Accept);

            MatchAndHandleSkill();

            flag = true;
        }

        return flag;
    }

    void MatchAndHandleSkill()
    {
        MatchSkill();

        // handle All Matched Skill


    }

    void onJudgmentEnd()
    {
        MatchSkill(true);
    }

    void MatchSkill(bool isEndJudgment = false)
    {
        SkillClip[] playerkillRegistries = GameManager.Instance.playerkillRegistries;

        matchedSkill.RemoveWhere((id) =>
        {
            SkillPhase[] phases = playerkillRegistries[id].phases;


            if (isEndJudgment)
            {
                if (phases.Length < slot + 1) return true;
                else
                {
                    if (phases[slot].inputClip.count <= 2) // 非缺省
                    {
                        if (phases[slot].inputClip != inputCache.bufferedInput[slot])
                        {
                            return true;
                        }
                        
                    }
                }



                return false;
            }


            if (phases.Length < slot + 1) return true;
            else //if (phases.Length == slot+1)
            {
                if (phases[slot].inputClip.count == 1)
                {
                    if (phases[slot].inputClip != inputCache.bufferedInput[slot])
                    {
                        return true;
                    }
                }
                else if (phases[slot].inputClip.count == 2)
                {
                    if (inputCache.bufferedInput[slot].count == 1) // 一次输入
                    {
                        if (inputCache.bufferedInput[slot].types[0] == PlayerInputBuffer.InputType.Accept) // Accept特判
                        {
                            if (phases[slot].inputClip.types[0] != PlayerInputBuffer.InputType.Accept
                            && phases[slot].inputClip.types[1] != PlayerInputBuffer.InputType.Accept)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (phases[slot].inputClip.types[0] != inputCache.bufferedInput[slot].types[0])
                            {
                                return true;
                            }
                        }
                    }
                    else // 两次输入
                    {
                        if (phases[slot].inputClip != inputCache.bufferedInput[slot])
                        {
                            return true;
                        }
                    }
                }
                else if (phases[slot].inputClip.count > 2) //缺省
                {

                }
                else // 原地不动
                {
                    return true;
                }

            }


            return false;
        });
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

            MatchAndHandleSkill();

        }
        if (Input.GetKeyDown(KeyCode.D)) //Move R
        {
            if (inputCache.GetBufferedCount(slot) == 1 && inputCache.GetBufferedType(slot, 0) == PlayerInputBuffer.InputType.Accept)
            {
                GridManager.Instance.playerAt += 1;
            }

            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveR);

            MatchAndHandleSkill();
        }
        if (Input.GetKeyDown(KeyCode.J)) //Atk`                             
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.Accept);

            MatchAndHandleSkill();
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
        if(GameManager.Instance.GetModifiedCurrBeat() == GameManager.Instance.currentBeat)
        {

        }
        else
        {
            inputCache.Clear();
            MatchAllSkill();
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
