using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private float health = 25f;

    void Update()
    {
        if (Input.GetAxis("Fire1") == 1)
        {
            GameObject camera = GameObject.FindWithTag("MainCamera");
            Vector3 rotation = new(camera.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
            GetComponent<BulletShoot>().Shoot(rotation);
        }
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        Debug.Log(health);
        health = Mathf.Max(health, 0);
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        Debug.Log(health);
    }
}
