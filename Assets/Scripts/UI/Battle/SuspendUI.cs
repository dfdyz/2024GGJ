using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuspendUI : MonoBehaviour
{
    [SerializeField] Button back_btn;
    [SerializeField] Button resume_btn;
    [SerializeField] Button restart_btn;
    // Start is called before the first frame update

    private void Awake()
    {
        back_btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MenuScene");
        });

        resume_btn.onClick.AddListener(() =>
        {
            UIManager.Instance.Resume();
        });

        restart_btn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("PlayScene");
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
