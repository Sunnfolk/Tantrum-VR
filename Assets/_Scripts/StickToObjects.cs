using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToObjects : MonoBehaviour
{
    private Rigidbody _rb;
    private MeshCollider _collider;
    // Start is called before the first frame update
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<MeshCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(_collider);
        Destroy(_rb);
        transform.parent = collision.transform;
    }
}
