using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSphere : MonoBehaviour
{
    private float healthAdd = 2f;
    private float lifetime = 5f;

    void Start()
    {
        StartCoroutine(Despawn());   
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            player.Heal(healthAdd);
            Destroy(gameObject);
        };
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
