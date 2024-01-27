

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillActionSheetSO", menuName = "ScriptableObject/SkillActionSheetSO", order = 1)]
public class SkillActionSheetSO : ScriptableObject
{
    
    public enum PhaseType
    {
        AttachMovement, Normal, AttachMovementWithAttack, Attack
    }

    [SerializeField]
    public SkillClip[] playerkillRegistries;


    [SerializeField]
    public SkillClip[] mobSkillRegistries;


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
         * 无效果 Normal
         *      不用填
         * 
         * 额外位移 AttachMovement
         *      0: 位移距离
         *      
         * 额外位移攻击 AttachMovementWithAttack
         *      0: 位移距离
         *      1: 伤害（得分？）
         * 
         * 攻击 Attack
         *      0: 攻击范围
         *      1: 伤害（得分？）
         * 
         */


        
        public PlayerInputBuffer.InputClip inputClip; 
        /*
            对于玩家：
                所有合法按键操作
            对于怪物：
                仅移动行为(不需要Accept)
         */


        public string audio;     // 特殊音效，留空则为默认

        public string effect;
    }









}