using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    [SerializeField] Text comboTxt;

    private void Update()
    {
        comboTxt.text = "" + GridManager.Instance.playerCtrl.combo;
    }

    

}
