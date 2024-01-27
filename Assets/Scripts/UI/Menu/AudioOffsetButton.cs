using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOffsetButton : MonoBehaviour
{
    private Button btn;
    [SerializeField]
    private Button btn_back;
    



    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SettingsManager.Instance.StartAdjustAudioOffset);
        btn_back.onClick.AddListener(SettingsManager.Instance.StopAdjust);
    }

    private void Update()
    {
        btn.enabled = !SettingsManager.Instance.beginAdjOff;
    }
}
