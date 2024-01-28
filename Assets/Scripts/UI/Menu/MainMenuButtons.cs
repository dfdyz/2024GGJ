using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SettingHolderSO settingData;
    [SerializeField] GameObject SettingPage;

    [SerializeField] Button[] backButtons;

    [SerializeField] Button SettingButton;
    [SerializeField] Button StartButton;
    [SerializeField] Button ExitButton;
    
    public static bool adjustedOff = false;

    void Start()
    {
        foreach (var button in backButtons)
        {
            button.onClick.AddListener(BackToMainPage);
        }

        SettingButton.onClick.AddListener(() =>
        {
            SettingPage.SetActive(true);
        });

        StartButton.onClick.AddListener(StartGame);

        ExitButton.onClick.AddListener(() => {
            Application.Quit();
        });

        if (!adjustedOff && !settingData.DebugMode)
        {
            SettingPage.SetActive(true);
        }


    }

    void StartGame()
    {
        CGManager.PlayCG(settingData, "AfterGameStart", "PlayScene");
    }

    void BackToMainPage()
    {
        SettingPage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
