using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public Transform Transform;

    private Rigidbody _Rigidbody;
    private float speed;

    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        speed = 10.0f;
    }

    private void Update()
    {
        _Rigidbody.velocity = new Vector3(0f,0f,speed);
    }
}
