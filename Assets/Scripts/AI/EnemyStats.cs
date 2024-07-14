using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float Health;
    public float MaxHealth;

    public int Attack;
    public int Defense;
    public int GoldDropped;

    public GameObject DeathEffect;
    GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void DeathCheck(ref int Gold)
    {
        int LootCheck = Random.Range(1,100);
        if (Health <= 0)
        {
            Gold = Random.Range(1, 100);
            Gold = Mathf.RoundToInt((Gold * GoldDropped)/100);
            if(LootCheck < 80)
            {
                GenerateLoot(LootCheck);
            }
            Destroy(Instantiate(DeathEffect,transform.position,Quaternion.identity),1);
            Destroy(gameObject);
        }
    }

    void GenerateLoot(int LootRoll)
    {
        int LootLevel = GameManager.Level;
        if (LootRoll > 90)
            LootLevel += 1;
        if (LootRoll > 95)
            LootLevel += 1;

        List<GameObject> LeveledItems = new List<GameObject>();
        foreach(GameObject Item in manager.LootTable)
        {
            if(Item.GetComponent<Equipment>().Level == LootLevel)
            {
                LeveledItems.Add(Item);
            }
        }
        Instantiate(LeveledItems[Random.Range(0,LeveledItems.Count)], transform.position, transform.rotation);
    }
}
