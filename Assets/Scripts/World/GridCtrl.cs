using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite Normal;
    [SerializeField] Sprite Danger;

    public int damage { get; private set; }
    public Vector2 LandingOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetDanger(int damage)
    {
        this.damage = damage;

        //spriteRenderer.color = damage <= 0 ? Color.white : Color.red;

        spriteRenderer.sprite = damage <= 0 ? Normal : Danger;
    }

    public void SetPosition(Vector2 vec2)
    {
        this.gameObject.transform.position = vec2;
    }

    public Vector2 GetLandingPos()
    {
        return (Vector2)this.gameObject.transform.position + LandingOffset;
    }


}
