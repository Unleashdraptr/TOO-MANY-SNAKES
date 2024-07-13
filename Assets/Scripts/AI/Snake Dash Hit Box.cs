using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDashHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            other.transform.GetChild(0).GetComponent<Player_Stats>().TakeDmg(transform.parent.GetComponent<EnemyStats>().Attack);
            other.GetComponent<Rigidbody>().velocity += transform.forward * 2 + transform.up * 1;
        }
    }
}
