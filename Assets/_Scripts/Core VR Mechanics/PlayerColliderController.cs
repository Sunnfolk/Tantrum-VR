using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderController : MonoBehaviour
{
    [SerializeField] private Transform _headset;
    [SerializeField] private Vector3 _colliderCenterOffset;
    [SerializeField] private Vector3 _worldSpaceOffset;

    private CapsuleCollider _collider;

    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        //get the heasdset position, add the local offset converted to localspace and add the worldspace offset
        Vector3 position = _headset.localPosition + _headset.TransformVector(_colliderCenterOffset) + _worldSpaceOffset;

        //set the collider height to the hedset+offsets height
        _collider.height = position.y;

        //set the collider center position to the headset+offsets x/z position and half of the y position
        _collider.center = new Vector3(position.x, _collider.height / 2, position.z);
    }
}
