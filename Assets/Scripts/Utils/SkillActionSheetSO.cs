

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillActionSheetSO", menuName = "ScriptableObject/SkillActionSheetSO", order = 1)]
public class SkillActionSheetSO : ScriptableObject
{
    
    public enum PhaseType
    {
        AttachMovement, Normal, AttachMovementWithAttac, Attack
    }

    [SerializeField]
    public SkillClip[] skillRegistries;

    [Serializable]
    public struct SkillClip
    {
        public string name;           //没啥屌用，可以空着，只是为了自己好区分
        public SkillPhase[] phases;
    }

    [Serializable]
    public struct SkillPhase
    {
        public PhaseType type;  // 段类型（额外位移  无特殊效果  额外位移且对路径造成伤害  攻击）

        public int[] phaseData;   // 段数据
        /*
         * 无效果
         *      不用填
         * 
         * 额外位移
         *      0: 位移距离
         *      
         * 额外位移攻击
         *      0: 位移距离
         *      1: 伤害（得分？）
         * 
         * 攻击
         *      0: 攻击范围
         *      1: 伤害（得分？）
         * 
         */

        public bool neededOrder; // 需要按顺序按下 input 中的按键
        public PlayerInputBuffer.InputType[] input;

        public string audio;     // 特殊音效，留空则为默认
    }
}