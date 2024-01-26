using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCtrl : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public int damage { get; private set; }

    

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
        spriteRenderer.color = damage <= 0 ? Color.white : Color.red;
    }

    public void SetPosition(Vector2 vec2)
    {
        this.gameObject.transform.position = vec2;
    }



}
