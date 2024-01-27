

using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillActionSheetSO", menuName = "ScriptableObject/SkillActionSheetSO", order = 1)]
public class SkillActionSheetSO : ScriptableObject
{
    
    public enum PhaseType
    {
        AttachMovement, Normal, AttachMovementWithAttack, Attack
    }

    [SerializeField]
    private SkillClip[] playerkillRegistries;


    [SerializeField]
    public SkillClip[] mobSkillRegistries;


    [Serializable]
    public struct SkillClip
    {
        public string name;           //没啥屌用，可以空着，只是为了自己好区分
        public SkillPhase[] phases;

        public SkillClip GetNeg()
        {
            SkillClip sc = new SkillClip();
            sc.name = name + "_neg";
            sc.phases = new SkillPhase[phases.Length];
            for (int i = 0; i < phases.Length; ++i)
            {
                sc.phases[i] = phases[i].GetNeg();
            }
            return sc;
        }
    }

    [Serializable]
    public struct SkillPhase
    {
        public PhaseType type;  // 段类型（额外位移  无特殊效果  额外位移且对路径造成伤害  攻击）

        public int[] phaseData;   // 段数据
        /*
         * 无效果 Normal
         *      不用填
         * 
         * 额外位移 AttachMovement
         *      0: 位移距离
         *      
         * 额外位移攻击 AttachMovementWithAttack
         *      0: 位移距离
         *      1: 伤害（得分？）
         *      2: 大于0则为打断敌人行动（仅玩家生效），不需要则不填
         * 
         * 攻击 Attack
         *      0: 攻击范围
         *      1: 伤害（得分？）
         *      2: 大于0则为打断敌人行动（仅玩家生效），不需要则不填
         */



        public PlayerInputBuffer.InputClip inputClip;
        /*
            对于玩家：
                types数组长度必须为2, count可以是 0 1 2 3，设置包含合法按键操作
                count 0:原地不动  1:一个按键  2:两个按键  3:缺省
            对于怪物：
                types数组里面必须有东西，仅读取第下标0
                Move   移动方向，attack朝向
                Accept 原地不动
         */


        public string audio;     // 特殊音效，留空则为默认
        public string effect;    // 特殊特效，留空则为默认

        public SkillPhase GetNeg()
        {
            SkillPhase sp = new SkillPhase();

            sp.type = type;
            sp.phaseData = new int[phaseData.Length];
            phaseData.CopyTo(sp.phaseData, 0);

            sp.inputClip = -inputClip;

            sp.audio = audio;
            sp.effect = effect;
            return sp;
        }
    }




    public SkillClip[] BakePlayerSkill()
    {
        SkillClip[] baked = new SkillClip[playerkillRegistries.Length * 2];
        for (int i = 0; i < playerkillRegistries.Length; ++i) {
            baked[2 * i] = playerkillRegistries[i];

            baked[2 * i + 1] = playerkillRegistries[i].GetNeg();
        }
        return baked;
    }
}