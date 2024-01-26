using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallow : MonoBehaviour
{
    [Header("Resources")]

    [Header("Parameters")]
    [SerializeField] float smooth = 0.2f;
    [SerializeField] GameObject A;
    [SerializeField] GameObject B;


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
        Vector3 posFocusOn = (A.transform.position + B.transform.position) / 2;
        posFocusOn = Vector3.Lerp(gameObject.transform.position, posFocusOn, smooth);
        posFocusOn.z = gameObject.transform.position.z;
        gameObject.transform.position = posFocusOn;
    }
}
