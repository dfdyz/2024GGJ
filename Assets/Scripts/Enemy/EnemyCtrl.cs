using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Resources")]


    [Header("Parameters")]
    [SerializeField] float moveSmooth = 0.25f;
    [Header("Display")]
    public int nextSkillBeat;
    public int faceDir = 1; // right
    public int currPhase = 0;

    int selfPos
    {
        get => GridManager.Instance.enemyAt;
        set => GridManager.Instance.enemyAt = value;
    }

    int playerPos
    {
        get => GridManager.Instance.playerAt;
    }

    private bool longSkill = false;

    public SkillActionSheetSO.SkillClip currentSkill;

    void Start()
    {
        GameManager.Instance.onHeavyBeat += onHeavyBeat;
        GameManager.Instance.onBeat += onBeat;

        GameManager.Instance.onGameStart += () =>
        {
            nextSkillBeat = 1;
        };

        GameManager.Instance.onJudgmentEndTick += delayJudgment;

        GridManager.Instance.enemyCtrl = this;


        gameObject.transform.position = GridManager.Instance.GetEnemyVisualPos();
        CameraFallow.ResetCamera();
    }



    // Update is called once per frame
    void Update()
    {
        MovementVisual();
        if (!GameManager.Instance.isStarted) return;
    }

    private void FixedUpdate()
    {

    }


    int stunBeginPhase = -100;
    public void Stun(bool hyper)
    {
        stunBeginPhase = hyper ? currPhase+1 : currPhase;
    }

    bool attackSucess = false;
    void onBeat()
    {
        if (currPhase == stunBeginPhase)
        {
            print("Been Stuned");
            currPhase = -10;
        }
        if(currPhase >= 0)
        {
            if (currPhase >= currentSkill.phases.Length)
            {
                currPhase = -10;
            }
            else  // handle action
            {
                SkillActionSheetSO.SkillPhase cPhase = currentSkill.phases[currPhase];
                switch (cPhase.type)
                {
                    case SkillActionSheetSO.PhaseType.Normal:

                        selfPos += MappingRealDir(cPhase.inputClip.types[0]);

                        break;
                    case SkillActionSheetSO.PhaseType.Attack:
                        if (MappingRealDir(cPhase.inputClip.types[0]) > 0) { // right
                            if (playerPos > selfPos && playerPos <= selfPos + cPhase.phaseData[0])
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                                attackSucess = true;
                            }
                        }
                        else // left
                        {
                            if (playerPos < selfPos && playerPos >= selfPos - cPhase.phaseData[0])
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                                attackSucess = true;
                            }
                        }

                        break;
                    case SkillActionSheetSO.PhaseType.AttachMovement:
                        selfPos += MappingRealDir(cPhase.inputClip.types[0]) * cPhase.phaseData[0];
                        break;
                    case SkillActionSheetSO.PhaseType.AttachMovementWithAttack:
                        // attack and move
                        if (MappingRealDir(cPhase.inputClip.types[0]) > 0)
                        { // right
                            if (playerPos >= selfPos && playerPos <= selfPos + cPhase.phaseData[0])
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                                attackSucess = true;
                            }
                        }
                        else // left
                        {
                            if (playerPos <= selfPos && playerPos >= selfPos - cPhase.phaseData[0])
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                                attackSucess = true;
                            }
                        }

                        selfPos += MappingRealDir(cPhase.inputClip.types[0]) * cPhase.phaseData[0];
                        break;
                    default: break;
                }
            }
            ++currPhase;
        }
    }

    void delayJudgment()
    {
        if (currPhase - 1 == stunBeginPhase)
        {
            print("Been Stuned");
            currPhase = -10;
        }

        if (currPhase >= 0)
        {
            SkillActionSheetSO.SkillPhase cPhase = currentSkill.phases[currPhase - 1];
            if (!attackSucess)
            {
                switch (cPhase.type)
                {
                    // no movement
                    case SkillActionSheetSO.PhaseType.Attack:
                        if (MappingRealDir(cPhase.inputClip.types[0]) > 0)
                        { // right
                            if (playerPos > selfPos && playerPos <= selfPos + cPhase.phaseData[0])
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                            }
                        }
                        else // left
                        {
                            if (playerPos < selfPos && playerPos >= selfPos - cPhase.phaseData[0])
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                            }
                        }


                        break;
                    case SkillActionSheetSO.PhaseType.AttachMovementWithAttack:
                        // attack and move
                        if (MappingRealDir(cPhase.inputClip.types[0]) > 0)
                        { // right
                            if (playerPos >= selfPos - cPhase.phaseData[0] && playerPos <= selfPos)
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);

                            }
                        }
                        else // left
                        {
                            if (playerPos <= selfPos + cPhase.phaseData[0] && playerPos >= selfPos)
                            {
                                GameManager.Instance.HurtPlayer(cPhase.phaseData[1]);
                            }
                        }

                        break;
                    default: break;
                }
            }
        }
        attackSucess = false;
    }

    public int GetNextPos()
    {
        if (currentSkill.phases.Length > currPhase) // ÐèÒªÔ¤²â
        {
            int predict = 0;
            SkillActionSheetSO.SkillPhase cPhase = currentSkill.phases[currPhase + 1];
            switch (cPhase.type)
            {
                case SkillActionSheetSO.PhaseType.Normal:
                    predict = MappingRealDir(cPhase.inputClip.types[0]);
                    break;
                case SkillActionSheetSO.PhaseType.AttachMovement:
                case SkillActionSheetSO.PhaseType.AttachMovementWithAttack:
                    predict = MappingRealDir(cPhase.inputClip.types[0]) * cPhase.phaseData[0];
                    break;
                default:
                    break;
            }

            return selfPos + predict;
        }
        else return selfPos;
  
    }

    public int MappingRealDir(PlayerInputBuffer.InputType type)
    {
        if(type == PlayerInputBuffer.InputType.MoveR)
        {
            return faceDir;
        }
        else if(type == PlayerInputBuffer.InputType.MoveL)
        {
            return -faceDir;
        }
        else {
            return 0;
        }
    }

    public void StunSelf()
    {
        currPhase = -10;
    }

    void onHeavyBeat()
    {
        if (GameManager.Instance.currentBeat == nextSkillBeat)
        {
            NextSkill();
        }
    }
    public void NextSkill()
    {
        int skill = UnityEngine.Random.Range(0, GameManager.Instance.mobSkillRegistries.Length);
        currentSkill = GameManager.Instance.mobSkillRegistries[skill];

        faceDir = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

        nextSkillBeat = currentSkill.phases.Length;
        if(nextSkillBeat % 4 != 0)
        {
            nextSkillBeat = (nextSkillBeat / 4 + 1) * 4;
        }
        nextSkillBeat += GameManager.Instance.currentBeat;
        currPhase = 0;
        stunBeginPhase = -100;
    }

    void MovementVisual()
    {
        Vector2 targetPos = GridManager.Instance.GetEnemyVisualPos();
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, targetPos, moveSmooth);
    }

}
