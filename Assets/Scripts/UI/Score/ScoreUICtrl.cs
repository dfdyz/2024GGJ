using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SettingHolderSO;

public class ScoreUICtrl : MonoBehaviour
{
    [SerializeField] SettingHolderSO settings;
    [SerializeField] PlayDataHolderSO data;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;
    // Start is called before the first frame update

    ScoreLevel GetLevel()
    {
        ScoreLevel l = settings.scoreLevels[0];
        for (int i = 1; i < settings.scoreLevels.Length; ++i) {
            if (settings.scoreLevels[i].scoreLine > data.Score) break;
            l = settings.scoreLevels[i];
        }
        return l;
    }

    void Start()
    {
        if(data.playerHealth > 0)
        {
            ScoreLevel l = GetLevel();
            levelText.text = l.Text;
        }
        else
        {
            levelText.text = "Lost";
        }
        scoreText.text = "" + data.Score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
