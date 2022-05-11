using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FireArm : MonoBehaviour
{
    [SerializeField] private Transform _trigger;
    private Vector3 _triggerStartPos;
    [SerializeField] private float _triggerMovementScale;
    [Space(5)]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _power;
    [SerializeField] private float _projectileLifetime;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private SteamVR_Action_Boolean _fireInput;
    [SerializeField] private SteamVR_Action_Single _triggerInput;

    private SteamVR_Input_Sources _currentInput;

    private bool _canFire;

    // Start is called before the first frame update
    void Start()
    {
        _currentInput = SteamVR_Input_Sources.Head;

        _triggerStartPos = _trigger.localPosition;
        _fireInput.AddOnStateDownListener(Fire, SteamVR_Input_Sources.RightHand);
        _fireInput.AddOnStateDownListener(Fire, SteamVR_Input_Sources.LeftHand);
    }

    private void OnDestroy()
    {
        _fireInput.RemoveOnStateDownListener(Fire, SteamVR_Input_Sources.RightHand);
        _fireInput.RemoveOnStateDownListener(Fire, SteamVR_Input_Sources.LeftHand);
    }

    private void FixedUpdate()
    {
        Vector3 _triggerMovement = new Vector3(0, _triggerInput.GetAxis(_currentInput) * _triggerMovementScale, 0);
        _triggerMovement = _trigger.TransformVector(_triggerMovement);
        _triggerMovement = transform.InverseTransformVector(_triggerMovement);

        _trigger.localPosition = _triggerStartPos + _triggerMovement;
    }

    private void Fire(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (_canFire && _currentInput == fromSource)
        {
            GameObject shot = Instantiate(_projectile, _firePoint.position, _firePoint.rotation);
            shot.GetComponent<Rigidbody>().velocity = shot.transform.up * _power;
            Destroy(shot, _projectileLifetime);
        }
    }

    public void CanFire(bool state, SteamVR_Input_Sources input)
    {
        _canFire = state;
        _currentInput = input;
    }
}
