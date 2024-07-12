using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrictorDashHitBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
}
