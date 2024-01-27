using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject SettingPage;

    [SerializeField] Button[] backButtons;

    [SerializeField] Button SettingButton;
    [SerializeField] Button StartButton;

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
    }

    void StartGame()
    {
        SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
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
