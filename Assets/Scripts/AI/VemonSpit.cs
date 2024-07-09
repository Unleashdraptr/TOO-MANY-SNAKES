using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomSpit : MonoBehaviour
{
    public GameObject VenomPuddle;

    private void OnCollisionEnter(Collision other)
    {
      if (LayerMask.NameToLayer("Ground") == other.gameObject.layer)
        {
            Instantiate(VenomPuddle).transform.position = transform.position;
            Destroy(gameObject);
        }

    }
}
