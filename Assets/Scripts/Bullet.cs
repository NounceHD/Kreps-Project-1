using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifetimeMax = 25f;
    private float speed = 3f;
    private bool hit = false;

    void Start()
    {
        StartCoroutine(Lifetime());
        GetComponent<Rigidbody>().AddForce(1000 * speed * transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hit)
        {
            hit = true;
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            EnemyCharacter enemy = other.GetComponent<EnemyCharacter>();
            if (player) player.Damage(1);
            if (enemy) enemy.Damage(1, transform.forward);
            Destroy(gameObject);
        }
        
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetimeMax);
        Destroy(gameObject);
    }
}
