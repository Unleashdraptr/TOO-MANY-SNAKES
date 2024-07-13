using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public int Attack;
    public int Defense;

    public GameObject DeathEffect;

    public void DeathCheck()
    { 
        if (Health <= 0)
        {
            Destroy(Instantiate(DeathEffect,transform.position,Quaternion.identity),1);
            Destroy(gameObject);
        }
    }
}
