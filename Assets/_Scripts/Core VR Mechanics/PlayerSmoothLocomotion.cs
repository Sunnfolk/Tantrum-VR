using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerSmoothLocomotion : MonoBehaviour
{
    [Header("0 = dissabled, 1 = controller oriented, 2 = headset oriented, 3 = playspace oriented")]
    [SerializeField] private int _movementDirection = 0;
    [Space(5)]
    [SerializeField] private Transform _headset;
    [SerializeField] private Transform _controller;
    [Space(10)]
    [SerializeField] private SteamVR_Action_Vector2 _moveInput;
    [SerializeField] private float _moveSpeed;
    
    private Rigidbody _rb;

    void FixedUpdate()
    {
        _rb = GetComponent<Rigidbody>();

        //get the movedirection from the input action, then convert it from x/y to x/z movement
        Vector3 moveDir = _moveInput.GetAxis(SteamVR_Input_Sources.Any);
        moveDir = new Vector3(moveDir.x, 0, moveDir.y);

        if (MenuInputManager.s_deviceIsUp)
        {
            moveDir = Vector3.zero;
        }


        //switch movement method
        switch (_movementDirection)
        {
            //controller oriented
            case 1:
                //convert moveDir from controller localspace to worldspace, then set y to 0
                moveDir = _controller.TransformVector(moveDir);
                moveDir.y = 0;

                //move player, convert moveDir from worldSpace to playspace localspace and add movespeed and deltatime
                transform.Translate(transform.InverseTransformVector(moveDir) * _moveSpeed * Time.deltaTime);
                break;

            //headset oriented
            case 2:
                //convert moveDir from headset localspace to worldspace, then set y to 0
                moveDir = _headset.TransformVector(moveDir);
                moveDir.y = 0;

                //move player, convert moveDir from worldSpace to playspace localspace and add movespeed and deltatime
                transform.Translate(transform.InverseTransformVector(moveDir) * _moveSpeed * Time.deltaTime);
                break;

            //playspace oriented
            case 3:
                //move the playspace based on the moveDir vector
                transform.Translate(moveDir * _moveSpeed * Time.deltaTime);
                break;
        }

        _rb.velocity = Vector3.zero;
    }
}
