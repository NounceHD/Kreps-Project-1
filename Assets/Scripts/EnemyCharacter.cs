using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] private GameObject healthSpherePrefab;

    public float health = 1f;
    private float speed = 5f;
    private bool targetPlayer = false;
    private float playerDistance = 0;
    private float rotationX = 0;
    private Vector3 velocity;
    private bool onGround = true;
    private float gravity = -9.8f;
    private int willingToFall = -1;

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
        CharacterController charCon = GetComponent<CharacterController>();

        if (onGround != charCon.isGrounded && charCon.isGrounded == false)
        {
            velocity.x = 0.5f * charCon.velocity.x;
            velocity.z = 0.5f * charCon.velocity.z;
        }
        onGround = charCon.isGrounded;

        Vector3 moveAmount = transform.forward;
        moveAmount = Vector3.ClampMagnitude(moveAmount * speed, speed);
        if (targetPlayer && playerDistance < 7) moveAmount *= -1;
        if (!onGround) moveAmount *= 0.5f;

        velocity.y += gravity * Time.deltaTime;
        if (onGround) velocity = new Vector3(0, -1f, 0);
        moveAmount += velocity;
        charCon.Move(moveAmount * Time.deltaTime);
    }
    private void DetectObstacle()
    {
        float distance = 2f;
        float size = 0.375f;
        Vector3 inFrontOffset = transform.forward * distance + Vector3.down;

        bool frontPath = Physics.SphereCast(transform.position, size, transform.forward, out RaycastHit hit, distance, LayerMask.GetMask("Default"));
        if (frontPath)
        {
            bool hitPlayer = false;
            if (hit.collider.GetComponent<PlayerCharacter>()) hitPlayer = hit.collider.GetComponent<PlayerCharacter>().isAlive;
            bool hitRamp = hit.collider.gameObject.CompareTag("Ramp");

            if (!hitPlayer && !hitRamp)
            {
                float randomRotate = Random.Range(-90f, 90f);
                transform.Rotate(0, randomRotate, 0);
            }
        } 

        GameObject player = GameObject.FindWithTag("Player");
        Vector3 playerDirection = player.transform.position - transform.position;
        playerDistance = playerDirection.magnitude;
        float angleBetween = Vector3.Angle(transform.forward, playerDirection);
        bool directLine = Physics.Raycast(transform.position, playerDirection, out RaycastHit hit1, Mathf.Infinity, LayerMask.GetMask("Default"));
        if (directLine)
        {
            bool hitPlayer1 = false;
            if (hit1.collider.GetComponent<PlayerCharacter>()) hitPlayer1 = hit1.collider.GetComponent<PlayerCharacter>().isAlive;

            if (angleBetween < 75 && hitPlayer1)
            {
                targetPlayer = true;
                Vector3 rotation = Quaternion.LookRotation(playerDirection, Vector3.up).eulerAngles;
                rotationX = rotation.x;
                rotation.x = 0;
                transform.rotation = Quaternion.Euler(rotation);
            } else
            {
                targetPlayer = false;
            }
        }
        
        if (!targetPlayer && !frontPath && !Physics.CheckSphere(transform.position + inFrontOffset, 0f))
        {
            if (willingToFall == -1)
            {
                StartCoroutine(madeDecision());
                willingToFall = Random.Range(0, 2);
            }
            if (willingToFall == 1)
            {
                float randomRotate = Random.Range(-90f, 90f);
                transform.Rotate(0, randomRotate, 0);
            }
        }
    }

    private IEnumerator madeDecision()
    {
        yield return new WaitForSeconds(5f);
        willingToFall = -1;
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
