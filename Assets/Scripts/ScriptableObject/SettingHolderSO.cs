using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SettingHolderSO", menuName = "ScriptableObject/SettingHolderSO", order = 2)]
public class SettingHolderSO : ScriptableObject
{
    public double audioOffset;
    public int subsectionCount = 24;

    public string playedCG = "";
    public string CGNextScene = "";

    public ScoreLevel[] scoreLevels;

    [Serializable]
    public struct ScoreLevel
    {
        public string Text;
        public int scoreLine;
    }

    public bool DebugMode = true;
}