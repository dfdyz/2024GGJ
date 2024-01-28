using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public static DebugManager Instance { get; private set; }

    public SettingHolderSO settings;
    public delegate void DebugWindowAttachment();

    public bool debugMode { get => settings.DebugMode; }

    public DebugWindowAttachment debugWindowAttachmentFunc = () => { };

    private void Awake()
    {
        Instance = this;
    }
    

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

        /*
        if (GUILayout.Button("AudioOffset"))
        {
            SettingsManager.Instance.StartAdjustAudioOffset();
        }*/

        GUILayout.Label(string.Format("Audio Delay: {0}", (int)(GameManager.Instance.audioOffset * 1000)));

        GUILayout.Label("Score: " + GameManager.Instance.gameData.Score);

        debugWindowAttachmentFunc();

        GUILayout.EndVertical();
        GUI.DragWindow();
    }

    Rect debugwinRect = new Rect(50, 50, 300, 800);
    private void OnGUI()
    {
        if(debugMode) debugwinRect = GUILayout.Window(114514, debugwinRect, DebugWindow, "Debug");
    }



}
