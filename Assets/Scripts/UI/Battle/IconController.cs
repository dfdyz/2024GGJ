using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Sprite[] spriteLevel;
    [SerializeField] int[] level;
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < level.Length; i++) {
            if(GameManager.Instance.gameData.playerHealth > level[i])
            {
                img.sprite = spriteLevel[i];
                break;
            }
        }
    }
}
