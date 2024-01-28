using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPredictor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputClipUI[] clipUIs;




    public InputClipUI GetSlot(int slot)
    {
        return clipUIs[slot];
    }

    
}
