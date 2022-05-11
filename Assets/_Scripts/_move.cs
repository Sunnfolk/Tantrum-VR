using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _move : MonoBehaviour
{

    public float _moveSpeed;
    Vector3 _moveDirection = Vector3.zero;
    Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {
        _moveDirection.x = Input.GetAxisRaw("Horizontal") * _moveSpeed;
        _moveDirection.z = Input.GetAxisRaw("Vertical") * _moveSpeed;
        _moveDirection.y = _rb.velocity.y;

        _rb.velocity = _moveDirection;

    }
}
