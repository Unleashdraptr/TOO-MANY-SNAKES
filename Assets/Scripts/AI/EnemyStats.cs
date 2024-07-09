using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public int Defense;

    public void DeathCheck()
    { 
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
