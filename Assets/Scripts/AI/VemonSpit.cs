using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpit : MonoBehaviour
{
    public GameObject VenomPuddle;
    public Vector3 Offset;

    private void OnCollisionEnter(Collision other)
    {
      if (LayerMask.NameToLayer("Ground") == other.gameObject.layer)
        {
            GameObject Puddle = Instantiate(VenomPuddle);
            Puddle.transform.position = transform.position + Offset;
            Puddle.transform.rotation = Quaternion.AngleAxis(Random.Range(0,180),Vector3.up) * Quaternion.AngleAxis(-90, Vector3.right);
            Destroy(Puddle, 5);   
            Destroy(gameObject);
        }

    }
}
