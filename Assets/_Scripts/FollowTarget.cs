using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _power = 100;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        _rb.MoveRotation(_target.rotation);

        Vector3 direction = _target.position - transform.position;

        _rb.velocity = direction * _power;
    }
}
