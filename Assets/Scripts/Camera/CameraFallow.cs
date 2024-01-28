using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    private static CameraFallow instance;
    [Header("Resources")]

    [Header("Parameters")]
    [SerializeField] float smooth = 0.2f;
    [SerializeField] Vector2 Offset = new Vector3 (0, 3, 0);


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateCamera();
    }

    private void FixedUpdate()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        Vector3 posFocusOn = (GridManager.Instance.GetPlayerVisualPos() + GridManager.Instance.GetEnemyVisualPos()) / 2 + Offset;
        posFocusOn = Vector3.Lerp(gameObject.transform.position, posFocusOn, smooth);
        posFocusOn.z = gameObject.transform.position.z;
        gameObject.transform.position = posFocusOn;
    }

    public static void ResetCamera()
    {
        Vector3 posFocusOn = (GridManager.Instance.GetPlayerVisualPos() + GridManager.Instance.GetEnemyVisualPos()) / 2 + instance.Offset;
        posFocusOn.z = instance.gameObject.transform.position.z;
        instance.gameObject.transform.position = posFocusOn;
    }
}
