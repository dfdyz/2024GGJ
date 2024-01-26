

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
        public string name;           //ûɶ���ã����Կ��ţ�ֻ��Ϊ���Լ�������
        public SkillPhase[] phases;
    }

    [Serializable]
    public struct SkillPhase
    {
        public PhaseType type;  // �����ͣ�����λ��  ������Ч��  ����λ���Ҷ�·������˺�  ������

        public int[] phaseData;   // ������
        /*
         * ��Ч��
         *      ������
         * 
         * ����λ��
         *      0: λ�ƾ���
         *      
         * ����λ�ƹ���
         *      0: λ�ƾ���
         *      1: �˺����÷֣���
         * 
         * ����
         *      0: ������Χ
         *      1: �˺����÷֣���
         * 
         */

        public bool neededOrder; // ��Ҫ��˳���� input �еİ���
        public PlayerInputBuffer.InputType[] input;

        public string audio;     // ������Ч��������ΪĬ��
    }
}