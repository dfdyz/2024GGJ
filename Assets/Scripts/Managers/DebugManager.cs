using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void DebugWindow(int id)
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("Start"))
        {
            GameManager.Instance.BattleStart();
        }
        if (GUILayout.Button("Stop"))
        {
            GameManager.Instance.BattleEnd();
        }

        if (GUILayout.Button("AudioOffset"))
        {
            Settings.Instance.StartAdjustAudioOffset();
        }

        GUILayout.Label(string.Format("{0:F3}", GameManager.Instance.audioOffset));

        GUILayout.EndVertical();

        GUI.DragWindow();
    }

    Rect debugwinRect = new Rect(50, 50, 300, 300);
    private void OnGUI()
    {
        debugwinRect = GUILayout.Window(114514, debugwinRect, DebugWindow, "Debug");
    }



}
