using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private float health = 25f;
    private Vector3 velocity;
    private float speed = 6f;
    private float gravity = -9.8f;
    private bool onGround = true;
    public bool isAlive = true;

    void Update()
    {
        if (!isAlive) return;

        Move();
        if (Input.GetAxis("Fire1") == 1)
        {
            GameObject camera = GameObject.FindWithTag("MainCamera");
            Vector3 rotation = new(camera.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            GetComponent<BulletShoot>().Shoot(rotation);
        }
    }

    private void Move()
    {
        CharacterController charCon = GetComponent<CharacterController>();
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float jump = Input.GetAxis("Jump");
        Vector3 moveAmount = new(0, 0, 0);

        if (onGround != charCon.isGrounded && charCon.isGrounded == false)
        {
            velocity.x = 0.5f * charCon.velocity.x;
            velocity.z = 0.5f * charCon.velocity.z;
        }
        onGround = charCon.isGrounded;

        moveAmount.x = horizontal;
        moveAmount.z = vertical;
        moveAmount = Vector3.ClampMagnitude(moveAmount * speed, speed);
        moveAmount = transform.TransformDirection(moveAmount);
        if (!onGround) moveAmount *= 0.5f;

        velocity.y += gravity * Time.deltaTime;
        if (onGround) velocity = new Vector3(0, -1f, 0);
        if (onGround && jump == 1) velocity.y = 4f;
        moveAmount += velocity;
        charCon.Move(moveAmount * Time.deltaTime);
    }

    private void Kill()
    {
        isAlive = false;
 
        Destroy(GetComponent<BulletShoot>());
        Debug.Log("Player has died");
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (health == 0) Kill();
        health = Mathf.Max(health, 0);
        Debug.Log(health);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        Debug.Log(health);
    }
}
