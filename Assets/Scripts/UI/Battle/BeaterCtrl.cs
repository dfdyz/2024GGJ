using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeaterCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject pointer;
    [SerializeField] Text count;

    [SerializeField] float smooth = 0.2f;

    [SerializeField] float targetAng = 0;
    [SerializeField] float currAng = 0;

    public void SetPointer(int pos)
    {
        float ang = pos * 90;

        if (targetAng > ang)
        {
            currAng -= 360;
        }

        targetAng = ang;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currAng = Mathf.Lerp(currAng, targetAng, smooth);
        pointer.transform.rotation = Quaternion.Euler(0, 0, -currAng);

        if (GameManager.Instance.isStarted)
        {
            if (GameManager.Instance.realStarted)
            {
                count.text = "" + (GameManager.Instance.settingData.subsectionCount - (GameManager.Instance.currentBeat - 1) / 4);
            }
            else
            {
                count.text = "" + GameManager.Instance.settingData.subsectionCount;
            }
        }
        else
        {
            count.text = "0";
        }
        
    }
}
