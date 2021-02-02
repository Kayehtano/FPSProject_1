using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public int maxEnemies;
    private int enemyCount = 0;

    private void Update()
    {
        if(enemyCount < maxEnemies)
        {
            StartCoroutine(Spawn());
        }
        else if (enemyCount == maxEnemies)
        {
            new WaitForSeconds(2f);
            enemyCount = 0;
        }
    }

    IEnumerator Spawn()
    {
        while (enemyCount < maxEnemies)
        {
            Instantiate(enemy, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            enemyCount++;
        }
    }
}
