using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private GameObject healthSpherePrefab;

    public float health = 1f;
    private float speed = 3f;
    private bool targetPlayer = false;
    private float playerDistance = 0;
    private float rotationX = 0;

    void Start()
    {
        

        float randomRotate = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, randomRotate, 0);
    }

    void Update()
    {
        if (health > 0)
        {
            Move();
            DetectObstacle();

            Vector3 rotation = new(rotationX, transform.rotation.eulerAngles.y, 0);
            if (targetPlayer) GetComponent<BulletShoot>().Shoot(rotation);
        }
    }

    private void Move()
    {
        Vector3 moveAmount = transform.forward;
        if (targetPlayer && playerDistance < 7) moveAmount *= -1;
        moveAmount = Vector3.ClampMagnitude(moveAmount * speed, speed);
        moveAmount.y = -9.8f;
        GetComponent<CharacterController>().Move(moveAmount * Time.deltaTime);
    }
    private void DetectObstacle()
    {
        float distance = 2f;

        bool frontPath = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distance);
        if (frontPath)
        {
            bool hitPlayer = false;
            if (hit.collider.GetComponent<PlayerCharacter>()) hitPlayer = hit.collider.GetComponent<PlayerCharacter>().isAlive;
            Bullet hitBullet = hit.collider.GetComponent<Bullet>();
            bool hitRamp = hit.collider.gameObject.CompareTag("Ramp");

            if (!hitBullet && !hitPlayer && !hitRamp)
            {
                float randomRotate = Random.Range(-90f, 90f);
                transform.Rotate(0, randomRotate, 0);
            }
        }

        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDistance = playerDirection.magnitude;
        float angleBetween = Vector3.Angle(transform.forward, playerDirection);
        bool directLine = Physics.Raycast(transform.position, playerDirection, out RaycastHit hit1);
        if (directLine)
        {
            bool hitPlayer1 = false;
            if (hit1.collider.GetComponent<PlayerCharacter>()) hitPlayer1 = hit1.collider.GetComponent<PlayerCharacter>().isAlive;
            Bullet hitBullet1 = hit1.collider.GetComponent<Bullet>();

            if (angleBetween < 75 && hitPlayer1)
            {
                targetPlayer = true;
                Vector3 rotation = Quaternion.LookRotation(playerDirection, Vector3.up).eulerAngles;
                rotationX = rotation.x;
                rotation.x = 0;
                transform.rotation = Quaternion.Euler(rotation);
            } else if (!hitBullet1)
            {
                targetPlayer = false;
            }
        }
    }
    private void Kill()
    {
        GetComponent<CharacterController>().enabled = false;
        Rigidbody rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.AddForce(50 * speed * transform.forward);

        GameObject healthSphere = Instantiate(healthSpherePrefab);
        healthSphere.transform.position = transform.position;
    }

    public void Damage(float damageAmount, Vector3 direction)
    {
        health -= damageAmount;
        if (health == 0) Kill();
        health = Mathf.Max(health, 0);
        if (health == 0) GetComponent<Rigidbody>().AddForce(direction * 200);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
    }
}
