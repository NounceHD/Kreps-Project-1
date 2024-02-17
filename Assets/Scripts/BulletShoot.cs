using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float cooldownMax = 0.1f;

    private bool canShoot = true;

    public void Shoot(Vector3 angle)
    {
        if (canShoot)
        {
            StartCoroutine(Cooldown());
            GameObject bullet = Instantiate(bulletPrefab);
            Vector3 position = (Quaternion.Euler(angle) * Vector3.forward * 1.2f) + transform.position;
            bullet.transform.SetPositionAndRotation(position, Quaternion.Euler(angle));
        }
    }

    private IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownMax);
        canShoot = true;
    }

    
}
