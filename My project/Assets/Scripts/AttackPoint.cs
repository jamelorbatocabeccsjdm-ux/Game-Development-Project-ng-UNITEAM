using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    public EntityStats entityStats;
    void OnTriggerEnter(Collider other)
    {
        EntityStats enemyStats = other.gameObject.GetComponent<EntityStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(entityStats.stats.attackPower);
        }
    }
}
