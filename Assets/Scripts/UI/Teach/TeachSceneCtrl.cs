using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachSceneCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SettingHolderSO setting;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CGManager.PlayCG(setting, "AfterTeach", "PlayScene");
        }
    }
}
