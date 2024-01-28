using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInputBuffer;

public class InputClipUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image img;
    [SerializeField] Image highLight;

    [SerializeField] Sprite None;
    [SerializeField] Sprite LL;
    [SerializeField] Sprite RR;
    [SerializeField] Sprite LJ;
    [SerializeField] Sprite RJ;
    [SerializeField] Sprite L;
    [SerializeField] Sprite R;
    [SerializeField] Sprite J;

    InputClip current = new InputClip {
        count = 0,
        types = new InputType[2]
    };

    public void SetClip(InputClip clip)
    {
        current = clip;
    }

    public void SetClip()
    {
        current = new InputClip
        {
            count = 0,
            types = new InputType[2]
        };
    }

    public void HighLight(int type)
    {
        Color col;

        if (type == 0) col = new Color(1, 1, 1, 0);  // none
        else if (type == 1) col = new Color(1, 1, 1, 0.5f); //inputed
        else col = new Color(0.5f, 1, 0.5f, 0.5f); // predict


        highLight.color = col;
    }

    public Sprite GetSprite()
    {
        if(current.count == 0) return None;
        else if(current.count == 1)
        {
            if (current.types[0] == InputType.Accept) return J;
            else if (current.types[0] == InputType.MoveL) return L;
            else return R;
        }
        else
        {
            if (current.types[1] == InputType.Accept)
            {
                if (current.types[0] == InputType.Accept) return J;
                else if (current.types[0] == InputType.MoveL) return LJ;
                else return RJ;
            }
            else
            {
                if (current.types[0] == InputType.Accept) return J;
                else if (current.types[0] == InputType.MoveL) return LL;
                else return RR;
            }
        }
    }

    private void Update()
    {
        img.sprite = GetSprite();
    }

}
