using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectHit : MonoBehaviour
{
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void TakeDamage(float damage)
    {
        if(this.name == "enemyHead")
        {
            enemy.health -= damage * 3f;
        }
        else if(this.name == "enemyTorso")
        {
            enemy.health -= damage;
        }
        else
        {
            enemy.health -= damage * 0.75f;
        }
    }
}
