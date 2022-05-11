using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkateboarMovement : MonoBehaviour
{
    [SerializeField] private Transform _headset;
    [SerializeField] private float _forwardMovement;
    [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(_headset.localPosition.x + _forwardMovement, 0, Mathf.Clamp(_headset.localPosition.z, -0.5f, 0.5f));

        if (moveDir.x < 0) moveDir.x = 0f;

        transform.Translate(moveDir * _speed * Time.deltaTime);
    }
}
