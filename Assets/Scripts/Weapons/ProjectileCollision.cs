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
            int Gold = 0;
            other.GetComponent<EnemyStats>().Health -= (Vector3.Magnitude(GetComponent<Rigidbody>().velocity)/2)- other.GetComponent<EnemyStats>().Defense; 
            other.GetComponent<EnemyStats>().DeathCheck(ref Gold);
            GameObject.Find("Player").GetComponent<Player_Stats>().Gold += Gold;
            Destroy(gameObject);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
