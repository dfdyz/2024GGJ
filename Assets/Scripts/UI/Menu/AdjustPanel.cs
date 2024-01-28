using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustPanel : MonoBehaviour
{
    
    [SerializeField] Text[] Off_sample_text = new Text[5];
    [SerializeField] GameObject textsHolder;
    [SerializeField] GameObject backButton;

    // Update is called once per frame
    void Update()
    {
        textsHolder.SetActive(SettingsManager.Instance.beginAdjOff);
        for (int i = 0; i < 4; ++i)
        {
            Off_sample_text[i].text = "" + (int)(SettingsManager.Instance.AdjOff_sample[i] * 1000);
        }

        Off_sample_text[4].text = "" + (int)(SettingsManager.Instance.settingData.audioOffset * 1000);

        backButton.SetActive(MainMenuButtons.adjustedOff);
    }
}
