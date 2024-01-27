using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Text scoreText;

    private void Update()
    {
        scoreText.text = "" + GameManager.Instance.gameData.Score;
    }

}
