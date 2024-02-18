using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private float wave = 0f;
    private bool fighting = false;

    void Start()
    {
        StartCoroutine(StartWave(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (fighting)
        {
            bool allDead = true;
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemys)
            {
                if (enemy.GetComponent<EnemyCharacter>().health > 0) allDead = false;
            }

            if (allDead && wave < 3)
            {
                StartCoroutine(StartWave(3));
            }

            if (allDead && wave == 3)
            {
                Debug.Log("Winner!");
                fighting = false;
            }
        }
    }

    private IEnumerator StartWave(float seconds)
    {
        fighting = false;
        yield return new WaitForSeconds(seconds);
        wave += 1;

        Vector3 enemySpawn = GameObject.FindWithTag("EnemySpawn").transform.position;
        for (int i = 0; i < wave; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = enemySpawn;
            enemy.GetComponent<EnemyCharacter>().health = wave;
        }

        fighting = true;
    }
}
