

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
        public string name;           //ûɶ���ã����Կ��ţ�ֻ��Ϊ���Լ�������
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
         *      2: ����0��Ϊ��ϵ����ж����������Ч��������Ҫ����
         * 
         * ���� Attack
         *      0: ������Χ
         *      1: �˺����÷֣���
         *      2: ����0��Ϊ��ϵ����ж����������Ч��������Ҫ����
         */



        public PlayerInputBuffer.InputClip inputClip;
        /*
            ������ң�
                types���鳤�ȱ���Ϊ2, count������ 0 1 2 3�����ð����Ϸ���������
                count 0:ԭ�ز���  1:һ������  2:��������  3:ȱʡ
            ���ڹ��
                types������������ж���������ȡ���±�0
                Move   �ƶ�����attack����
                Accept ԭ�ز���
         */


        public string audio;     // ������Ч��������ΪĬ��
        public string effect;    // ������Ч��������ΪĬ��

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