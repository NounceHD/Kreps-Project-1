using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    private float speed = 6f;
    private float horizontal;
    private float vertical;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 moveAmount = new(horizontal, 0, vertical);
        moveAmount = Vector3.ClampMagnitude(moveAmount * speed, speed);
        moveAmount = transform.TransformDirection(moveAmount);
        moveAmount.y = -9.8f;
        GetComponent<CharacterController>().Move(moveAmount * Time.deltaTime);
    }
}
