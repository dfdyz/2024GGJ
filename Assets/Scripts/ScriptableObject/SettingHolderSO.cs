using UnityEngine;

[CreateAssetMenu(fileName = "SettingHolderSO", menuName = "ScriptableObject/SettingHolderSO", order = 2)]
public class SettingHolderSO : ScriptableObject
{
    public double audioOffset;


    public bool DebugMode = true;
}