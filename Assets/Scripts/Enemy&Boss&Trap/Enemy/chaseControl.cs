using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaseControl : MonoBehaviour
{

    public EnemyFlying[] enemyArray;
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            foreach (EnemyFlying enemy in enemyArray)
            {
                enemy.chase = true;
            }
        }
    }    
    
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            foreach (EnemyFlying enemy in enemyArray)
            {
                enemy.chase = false;
            }
        }
    }

}
