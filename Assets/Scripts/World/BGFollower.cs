using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGFollower : MonoBehaviour
{
    [Header("Resources")]

    [Header("Parameters")]
    [SerializeField] float smooth = 0.2f;

    // Start is called before the first frame update

    void Start()
    {
        gameObject.transform.position = GridManager.Instance.GetEnemyVisualPos();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posFocusOn = GridManager.Instance.GetEnemyVisualPos();
        posFocusOn = Vector3.Lerp(gameObject.transform.position, posFocusOn, smooth);
        posFocusOn.z = gameObject.transform.position.z;
        gameObject.transform.position = posFocusOn;
    }
}
