using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    public void bulletShoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.SetPositionAndRotation(transform.TransformPoint(Vector3.forward * 1.2f), transform.rotation);
    }
}
