

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
        public string name;           //ûɶ���ã����Կ��ţ�ֻ��Ϊ���Լ�������
        public SkillPhase[] phases;
    }

    [Serializable]
    public struct SkillPhase
    {
        public PhaseType type;  // �����ͣ�����λ��  ������Ч��  ����λ���Ҷ�·������˺�  ������

        public int[] phaseData;   // ������
        /*
         * ��Ч�� Normal
         *      ������
         * 
         * ����λ�� AttachMovement
         *      0: λ�ƾ���
         *      
         * ����λ�ƹ��� AttachMovementWithAttack
         *      0: λ�ƾ���
         *      1: �˺����÷֣���
         * 
         * ���� Attack
         *      0: ������Χ
         *      1: �˺����÷֣���
         * 
         */


        
        public PlayerInputBuffer.InputClip inputClip; 
        /*
            ������ң�
                ���кϷ���������
            ���ڹ��
                ���ƶ���Ϊ(����ҪAccept)
         */


        public string audio;     // ������Ч��������ΪĬ��

        public string effect;
    }









}