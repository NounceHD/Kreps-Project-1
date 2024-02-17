using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LookControl : MonoBehaviour
{
    private enum RotationAxis {
        MouseX= 0,
        MouseY= 1
    }

    [SerializeField] private RotationAxis state = RotationAxis.MouseY;

    private float YSens = 9.0f;
    private float XSens = 9.0f;
    private float Ymin = -90.0f;
    private float Ymax = 90.0f;
    private float rotationY = 0.0f;
    private float rotationX = 0.0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (state == RotationAxis.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * XSens;
            transform.localEulerAngles = new Vector3(0, rotationX, 0);
        } else
        {
            rotationY -= Input.GetAxis("Mouse Y") * YSens;
            rotationY = Mathf.Clamp(rotationY, Ymin, Ymax);
            transform.localEulerAngles = new Vector3(rotationY, 0, 0);

        }
    }
}
