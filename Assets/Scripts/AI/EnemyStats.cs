using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public int Attack;
    public int Defense;
    public int GoldDropped;

    public GameObject DeathEffect;

    public void DeathCheck(ref int Gold)
    {
        if (Health <= 0)
        {
            Gold = Random.Range(1, 100);
            Gold = Mathf.RoundToInt((Gold * GoldDropped)/100);
            Destroy(Instantiate(DeathEffect,transform.position,Quaternion.identity),1);
            Destroy(gameObject);
        }
    }
}
