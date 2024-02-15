using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookControl : MonoBehaviour
{
    private float YSens = 9.0f;
    private float XSens = 9.0f;
    private float Ymin = -90.0f;
    private float Ymax = 90.0f;
    public float rotationY = 0.0f;
    public float rotationX = 0.0f;

    // Update is called once per frame
    void Update()
    {
            rotationY -= Input.GetAxis("Mouse Y") * YSens;
            rotationY = Mathf.Clamp(rotationY, Ymin, Ymax);
            rotationX += Input.GetAxis("Mouse X") * XSens;
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
    }
}
