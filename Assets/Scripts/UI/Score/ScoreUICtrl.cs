using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SettingHolderSO;

public class ScoreUICtrl : MonoBehaviour
{
    [SerializeField] SettingHolderSO settings;
    [SerializeField] PlayDataHolderSO data;

    [SerializeField] Button backButton;
    [SerializeField] Button nextButton;

    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;

    [SerializeField] Image playerImg;
    [SerializeField] Image enemyImg;


    [SerializeField] Sprite[] playerSpriteLevel;
    [SerializeField] Sprite[] enemySpriteLevel;
    [SerializeField] int[] healthLevel;


    // Start is called before the first frame update

    int GetScoreLevel()
    {
        int l = 0;
        for (int i = 1; i < settings.scoreLevels.Length; ++i) {
            if (settings.scoreLevels[i].scoreLine > data.Score) break;
            l = i;
        }
        return l;
    }



    void Start()
    {
        backButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        });


        nextButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
        });


        int sl = GetScoreLevel();
        enemyImg.sprite = enemySpriteLevel[sl];

        if (data.playerHealth > 0)
        {
            ScoreLevel l = settings.scoreLevels[sl];
            levelText.text = l.Text;
        }
        else
        {
            levelText.text = "Lost";
        }
        scoreText.text = "" + data.Score;

        playerImg.sprite = playerSpriteLevel[0];
        for (int i = 1; i < healthLevel.Length; i++)
        {
            if (data.playerHealth > healthLevel[i])
            {
                playerImg.sprite = playerSpriteLevel[i];
                break;
            }
        }
    }

    // Update is called once per frame

    


    void Update()
    {
        
    }
}
