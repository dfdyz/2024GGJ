using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] RectTransform mask;
    [SerializeField] RectTransform hp;

    public float rate = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hp.anchoredPosition = new Vector2(Mathf.Clamp(rate - 1, -1, 0) * mask.rect.width, 0);
    }
}
