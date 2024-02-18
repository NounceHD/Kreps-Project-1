using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private float health = 25f;
    public bool isAlive = true;

    void Update()
    {
        if (Input.GetAxis("Fire1") == 1)
        {
            GameObject camera = GameObject.FindWithTag("MainCamera");
            Vector3 rotation = new(camera.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            GetComponent<BulletShoot>().Shoot(rotation);
        }
    }
    private void Kill()
    {
        isAlive = false;
        Destroy(GetComponent<MovementControl>());
        Destroy(GetComponent<BulletShoot>());
        Debug.Log("Player has died");
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (health == 0) Kill();
        health = Mathf.Max(health, 0);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
    }
}
