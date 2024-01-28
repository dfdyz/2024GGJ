using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] protected float smoothTime = 0.4f;
    [SerializeField] int defaultFace = 1;

    protected float currentTime = 0;
    
    protected virtual void Start()
    {
        DebugManager.Instance.debugWindowAttachmentFunc += DebugInfo;
    }

    void DebugInfo()
    {
        if (GUILayout.Button(gameObject.name))
        {
            ShowEffect(8, -1);
        }
    }


    public virtual void ShowEffect(int pos, float face)
    {
        gameObject.transform.position = GridManager.Instance.GetViaualPosAt(pos);
        gameObject.transform.localScale = new Vector3(defaultFace * face, 1, 1);
        currentTime = smoothTime;
    }


    protected virtual void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        
        currentTime = Mathf.Max(currentTime, 0);

        foreach (var sprite in sprites)
        {
            Color color = sprite.color;
            color.a = currentTime / smoothTime;
            sprite.color = color;
        }
    }

}
