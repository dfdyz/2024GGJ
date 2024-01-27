using UnityEngine;

[CreateAssetMenu(fileName = "PlayDataHolderSO", menuName = "ScriptableObject/PlayDataHolderSO", order = 4)]
public class PlayDataHolderSO : ScriptableObject
{
    public int playerHealth = 1;
    public int Score = 0;
    public int maxCombo = 0;

    public void Combo(int combo)
    {
        maxCombo = Mathf.Max(maxCombo, combo);
    }

    public void ResetData()
    {
        playerHealth = 1;
        Score = 0;
        maxCombo = 0;
    }
}