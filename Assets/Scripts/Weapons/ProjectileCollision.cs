using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    float Timer = 10;
    private void Update()
    {
        Timer -= Time.deltaTime;
        if(Timer < 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Player_Stats stats = GameObject.Find("Player").GetComponent<Player_Stats>();
            int Gold = 0;
            float Dmg = (Vector3.Magnitude(GetComponent<Rigidbody>().velocity) / 2) - other.GetComponent<EnemyStats>().Defense;
            if (Random.Range(1,100) < stats.CritChance)
            {
                Dmg *= stats.CritMult;
            }
            other.GetComponent<EnemyStats>().Health -= Dmg; 
            other.GetComponent<EnemyStats>().DeathCheck(ref Gold);
            stats.Gold += Gold;
            Destroy(gameObject);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
