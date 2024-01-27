using UnityEngine;

[CreateAssetMenu(fileName = "PlayDataHolderSO", menuName = "ScriptableObject/PlayDataHolderSO", order = 4)]
public class PlayDataHolderSO : ScriptableObject
{
    public int Score = 0;

    public int maxCombo = 0;

    public void ResetData()
    {
        Score = 0;
        maxCombo = 0;
    }
}