using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parenter : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.parent != transform)
        {
            collision.transform.parent = transform;
            if(collision.GetComponent<Rigidbody>() != null)
            {
                Destroy(collision.GetComponent<Rigidbody>(), 3);
            }
        }
    }
}
