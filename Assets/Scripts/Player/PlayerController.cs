using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static SkillActionSheetSO;
using static UnityEditor.ShaderData;

public class PlayerController : MonoBehaviour
{
    [Header("Resources")]
    

    [Header("Parameters")]
    [SerializeField] int MaxHealth = 100;

    [SerializeField] float moveSmooth = 0.25f;

    [Header("Display")]
    public HashSet<int> matchedSkill = new HashSet<int>();
    public int combo = 0;
    public int faceDir = 1;

    private bool skillHandled = false;

    private PlayerInputBuffer inputCache = new PlayerInputBuffer(8);


    public int Health
    {
        get => GameManager.Instance.gameData.playerHealth;
        set => GameManager.Instance.gameData.playerHealth = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.onBeat = onBeat;
        GameManager.Instance.onHeavyBeat = onHeavyBeat;
        GameManager.Instance.onNormalBeat = onNormalBeat;
        GameManager.Instance.onJudgmentEndTick = onJudgmentEnd;

        GameManager.Instance.onGameStart += () =>
        {
            Health = MaxHealth;
        };

        DebugManager.Instance.debugWindowAttachmentFunc += DebugInfo;

        CameraFallow.ResetCamera();
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

        GUILayout.Label("Combo: "+combo);
    }

    public void ResetPlayer()
    {
        Health = MaxHealth;
    }


    public void ShootEffect()
    {

    }


    // Update is called once per frame

    int judgmentBeat = 0;
    void Update()
    {
        MovementVisual();
        if (!GameManager.Instance.isStarted || !GameManager.Instance.realStarted) return;

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
                        //print("Beated");
                    }
                }
                else     //second
                {
                    HandleJudgmentSecond();
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.J))
                {
                    combo = 0;
                    GameManager.Instance.HurtPlayer(10);
                }
            }
        }
    }

    [SerializeField]
    bool attachMatch = false;

    void ResetMatch()
    {
        matchedSkill.Clear();
        for (int i = 0; i < GameManager.Instance.playerSkillRegistries.Length; i++)
        {
            matchedSkill.Add(i);
        }
        skillHandled = false;
    }

    int slot;
    bool HandleJudgmentFirst()
    {
        //print(judgmentBeat + " " + GameManager.Instance.currentBeat);
        slot = (judgmentBeat - 1) % 4 + (attachMatch ? 4:0);

        GameManager.Instance.gameData.Combo(++combo);
        GameManager.Instance.HurtPlayer(-2);
        GameManager.Instance.gameData.playerHealth = Mathf.Clamp(GameManager.Instance.gameData.playerHealth, 0, MaxHealth);

        bool attach = slot == 0 && judgmentBeat > GameManager.Instance.currentBeat && matchedSkill.Count > 0;
        slot += attach && slot < 4 ? 4 : 0;

        if (slot == 0) //clear input cache
        {
            inputCache.Clear();
            ResetMatch();
        }

        bool flag = false;

        if (Input.GetKeyDown(KeyCode.A)) //Move L
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveL);

            selfPos -= 1;
            faceDir = -1;

            MatchAndHandleSkill();

            flag = true;
        }
        if (Input.GetKeyDown(KeyCode.D)) //Move R
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveR);

            selfPos += 1;

            faceDir = 1;

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

        if(slot >= 4) // 溢出
        {
            if(matchedSkill.Count > 0) // 正确连招
            {
                attachMatch = true;
            }
            else   // 错误连招
            {
                attachMatch = false;
                if (inputCache.bufferedCount >= 4)  // 炸了需要重开
                {
                    inputCache.MoveForward();
                    slot %= 4;

                    ResetMatch();
                    ReMatchSkill();
                }


 

            }
        }
        else
        {
            

            
        }
        // handle All Matched Skill

        if (!skillHandled)
        {
            foreach (int id in matchedSkill)
            {
                SkillClip sc = GameManager.Instance.playerSkillRegistries[id];

                SkillPhase phase = sc.phases[slot];
                if (phase.inputClip == inputCache.bufferedInput[slot])
                {
                    if (slot + 1 == sc.phases.Length)// skill
                    {
                        skillHandled = true;
                    }
                    HandlePhase(phase);
                }
            }
        }
    }

    int selfPos { 
        get => GridManager.Instance.playerAt;
        set => GridManager.Instance.playerAt = value;
    }
    int enemyPos { 
        get
        {
            if (isHyperInput()) //超前
            {
                return GridManager.Instance.enemyCtrl.GetNextPos();
            }
            return GridManager.Instance.enemyAt;
        }
    }
    bool isHyperInput()
    {
        return GameManager.Instance.lastSucessBeat > GameManager.Instance.currentBeat;
    }
    float GetScoreUpper(int step)
    {
        int level = combo / step;
        return 1.0f + (0.1f * level);
    }

    void HandlePhase(SkillPhase phase)
    {
        //print(phase.type);
        switch(phase.type)
        {
            case PhaseType.Attack:
                if (phase.effect != "")
                {
                    EffectManager.GetEffectInctanceByName(phase.effect).ShowEffect(selfPos, faceDir);
                }
                if (faceDir > 0)
                { // right
                    if (enemyPos >= selfPos && enemyPos <= selfPos + phase.phaseData[0])
                    {
                        GameManager.Instance.AddScore((int)(GetScoreUpper(4) * phase.phaseData[1]));
                        if(phase.phaseData.Length >= 3) // stun enemy
                        {
                            GridManager.Instance.enemyCtrl.Stun(isHyperInput());
                        }
                    }
                }
                else // left
                {
                    if (enemyPos <= selfPos && enemyPos >= selfPos - phase.phaseData[0])
                    {
                        GameManager.Instance.AddScore((int)(GetScoreUpper(4) * phase.phaseData[1]));
                        if (phase.phaseData.Length >= 3) // stun enemy
                        {
                            GridManager.Instance.enemyCtrl.Stun(isHyperInput());
                        }
                    }
                }

                break;

            case PhaseType.AttachMovement:
                selfPos += faceDir * phase.phaseData[0];
                break;

            case PhaseType.AttachMovementWithAttack:
                if (phase.effect != "")
                {
                    EffectManager.GetEffectInctanceByName(phase.effect).ShowEffect(selfPos, faceDir);
                }

                selfPos += faceDir * phase.phaseData[0];

                if (faceDir > 0)
                { // right
                    if (enemyPos >= selfPos - phase.phaseData[0] - 1 && enemyPos <= selfPos)
                    {
                        GameManager.Instance.AddScore((int)(GetScoreUpper(4) * phase.phaseData[1]));
                        if (phase.phaseData.Length >= 3) // stun enemy
                        {
                            GridManager.Instance.enemyCtrl.Stun(isHyperInput());
                        }
                    }
                }
                else // left
                {
                    if (enemyPos <= selfPos + phase.phaseData[0] + 1 && enemyPos >= selfPos)
                    {
                        GameManager.Instance.AddScore((int)(GetScoreUpper(4) * phase.phaseData[1]));
                        if (phase.phaseData.Length >= 3) // stun enemy
                        {
                            GridManager.Instance.enemyCtrl.Stun(isHyperInput());
                        }
                    }
                }

                break;
            case PhaseType.Normal:
            default:
                break;
        }

        
    }



    void onJudgmentEnd()
    {
        // 匹配不动
        MatchSkill(true);
        if(slot > 3 && matchedSkill.Count > 0) // 溢出检测
        {
            attachMatch = true;
        }

        if(attachMatch && matchedSkill.Count == 0) // 溢出炸了需要重开
        {
            inputCache.MoveForward();
            attachMatch = false;
            ResetMatch();
            ReMatchSkill();
        }

        if (GameManager.Instance.isStarted)
        {
            GridCtrl gc = GridManager.Instance.GetPlayerCurrentGrid();

            UIManager.Instance.ShowDangerEffect(gc.damage > 0);

            if (gc.damage > 0)
            {
                GameManager.Instance.HurtPlayer(gc.damage);
            }
        }

    }

    #region Folder1
    void ReMatchSkill()
    {
        SkillClip[] playerkillRegistries = GameManager.Instance.playerSkillRegistries;
        matchedSkill.RemoveWhere((id) =>
        {
            SkillPhase[] phases = playerkillRegistries[id].phases;

            if (phases.Length < slot + 1) return true;

            for (int i = 0; i <= inputCache.bufferedCount; ++i)
            {
                if(ShouldDeleteSingle(phases[i], i))
                {
                    return true;
                }
            }
            return false;
        });
    }
    public bool ShouldDeleteSingle(SkillPhase phase, int bufferSlot)
    {
        if (phase.inputClip.count == 1)
        {
            if (phase.inputClip != inputCache.bufferedInput[bufferSlot])
            {
                return true;
            }
        }
        else if (phase.inputClip.count == 2)
        {
            if (inputCache.bufferedInput[bufferSlot].count == 0) return true;
            if (inputCache.bufferedInput[bufferSlot].count == 1) // 已经一次输入
            {
                if (inputCache.bufferedInput[bufferSlot].types[0] == PlayerInputBuffer.InputType.Accept) // Accept特判
                {
                    if (phase.inputClip.types[0] != PlayerInputBuffer.InputType.Accept
                    && phase.inputClip.types[1] != PlayerInputBuffer.InputType.Accept)
                    {
                        //print("One_ACC");
                        return true;
                    }
                }
                else
                {
                    if (phase.inputClip.types[0] != inputCache.bufferedInput[bufferSlot].types[0])
                    {
                        //print("One: " + phase.inputClip.types[0] + " " + inputCache.bufferedInput[bufferSlot].types[0] + " " + slot);
                        return true;
                    }
                }
            }
            else // 两次输入
            {
                if (phase.inputClip != inputCache.bufferedInput[bufferSlot])
                {
                    print("Double: " + phase.inputClip.GetHashCode() + " " + inputCache.bufferedInput[bufferSlot].GetHashCode() + " " + slot);
                    return true;
                }
            }
        }
        else if (phase.inputClip.count > 2) //缺省
        {
            return false;
        }
        else // 原地不动
        {
            if (inputCache.bufferedInput[bufferSlot].GetHashCode() != 0)
            {
                return true;
            }
        }


        return false;
    }

    void MatchSkill(bool isEndJudgment = false)
    {
        SkillClip[] playerkillRegistries = GameManager.Instance.playerSkillRegistries;

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
            else return ShouldDeleteSingle(phases[slot], slot);
        });
    }

    void HandleJudgmentSecond()
    {
        //slot = (judgmentBeat - 1) % 4;
        if (Input.GetKeyDown(KeyCode.A)) //Move L
        {
            if (inputCache.GetBufferedCount(slot) == 1 && inputCache.GetBufferedType(slot, 0) == PlayerInputBuffer.InputType.Accept)
            {
                selfPos -= 1;
                faceDir = -1;
            }

            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveL);

            MatchAndHandleSkill();

        }
        if (Input.GetKeyDown(KeyCode.D)) //Move R
        {
            if (inputCache.GetBufferedCount(slot) == 1 && inputCache.GetBufferedType(slot, 0) == PlayerInputBuffer.InputType.Accept)
            {
                selfPos += 1;
                faceDir = 1;
            }

            inputCache.Put(slot, PlayerInputBuffer.InputType.MoveR);

            MatchAndHandleSkill();
        }
        if (Input.GetKeyDown(KeyCode.J)) //Atk                       
        {
            inputCache.Put(slot, PlayerInputBuffer.InputType.Accept);

            MatchAndHandleSkill();
        }
    }

    void onBeat()
    {
        
    }

    #endregion

    void onHeavyBeat()
    {
        // 没超前操作
        if (GameManager.Instance.lastSucessBeat < GameManager.Instance.currentBeat)
        {
            ++slot;
            if (matchedSkill.Count > 0) // 还有连招
            {
                attachMatch = true;
                inputCache.ResetSlotBehind(slot);
                //inputCache.PutBufferEmpty(slot);
            }
            else // 没连招了
            {
                inputCache.Clear();
                ResetMatch();
                slot = 0;
            }
        }


        if (GameManager.Instance.lastSucessBeat == GameManager.Instance.currentBeat)
        {

        }
        else
        {
            // 没超前操作
            

        }
    }

    void onNormalBeat()
    {
        if (GameManager.Instance.lastSucessBeat < GameManager.Instance.currentBeat) // 没有超前操作
        {
            ++slot;
            inputCache.ResetSlotBehind(slot);
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
