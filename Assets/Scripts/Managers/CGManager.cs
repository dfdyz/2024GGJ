using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CGManager : MonoBehaviour
{
    [SerializeField] SettingHolderSO setting;
    [SerializeField] VideoPlayer player;
    // Start is called before the first frame update

    private void Awake()
    {

    }

    private void Start()
    {
        StartCoroutine(PlayCG());
    }


    IEnumerator PlayCG()
    {
        VideoClip clip = null;
        clip = Resources.Load<VideoClip>("CG/" + setting.playedCG);
        if (clip != null)
        {
            player.clip = clip;
            player.Play();
            yield return new WaitForSeconds((float)(clip.length + 0.1));
        }
        else
        {
            yield return null;

        }
        if (setting.CGNextScene == "")
        {
            SceneManager.LoadScene("MenuScene");
        }
        else
        {
            SceneManager.LoadScene(setting.CGNextScene);
        }
    }

    public static void PlayCG(SettingHolderSO settingData, string path, string nextScene)
    {
        settingData.playedCG = path;
        settingData.CGNextScene = nextScene;
        SceneManager.LoadScene("CGScene");
    }

}
