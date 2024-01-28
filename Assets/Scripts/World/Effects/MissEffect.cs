using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissEffect : EffectBase
{
    [SerializeField] Text txt;
    [SerializeField] AnimationCurve curve;

    // Start is called before the first frame update
    protected override void Start()
    {
        
    }

    public override void ShowEffect(int pos, float face)
    {
        currentTime = smoothTime;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        currentTime = Mathf.Max(currentTime, 0);
            Color color = txt.color;
            color.a = curve.Evaluate(currentTime / smoothTime);
            txt.color = color;
        
    }
}
