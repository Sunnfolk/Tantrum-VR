using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerRotation : MonoBehaviour
{
    [Header("0 = none, 1 = snap, 2 = smooth")]
    public int turnMode = 1;
    [Space(5)]
    public float snapAngle = 45;
    public float smoothTurnSpeed = 1.5f;
    [Space(10)]
    [SerializeField] private SteamVR_Action_Boolean _turnRight;
    [SerializeField] private SteamVR_Action_Boolean _turnLeft;
    [Space(5)]
    [SerializeField] private Transform _headset;
    [SerializeField] private Vector3 _rotationCenterOffset;

    private float _rotate;

    // Start is called before the first frame update
    void Start()
    {
        _turnLeft.AddOnStateDownListener(TurnLeftPress, SteamVR_Input_Sources.Any);
        _turnLeft.AddOnStateUpListener(TurnRelease, SteamVR_Input_Sources.Any);
        _turnRight.AddOnStateDownListener(TurnRightPress, SteamVR_Input_Sources.Any);
        _turnRight.AddOnStateUpListener(TurnRelease, SteamVR_Input_Sources.Any);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (turnMode == 2 && _rotate != 0)
        {
            transform.RotateAround(_headset.position + (_headset.TransformVector(_rotationCenterOffset) * transform.localScale.x), transform.up, _rotate);
        }
    }

    void TurnLeftPress(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!MenuInputManager.s_deviceIsUp)
        {
            switch (turnMode)
            {
                case 1:
                    transform.RotateAround(_headset.position + (_headset.TransformVector(_rotationCenterOffset) * transform.localScale.x), transform.up, -snapAngle);
                    break;
                case 2:
                    _rotate = -smoothTurnSpeed;
                    break;
            }
        }
    }

    void TurnRightPress(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (!MenuInputManager.s_deviceIsUp)
        {
            switch (turnMode)
            {
                case 1:
                    transform.RotateAround(_headset.position + (_headset.TransformVector(_rotationCenterOffset) * transform.localScale.x), transform.up, snapAngle);
                    break;
                case 2:
                    _rotate = smoothTurnSpeed;
                    break;
            }
        }
    }

    void TurnRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(turnMode == 2)
        {
            _rotate = 0;
        }
    }

    private void OnDestroy()
    {
        _turnLeft.RemoveOnStateDownListener(TurnLeftPress, SteamVR_Input_Sources.Any);
        _turnLeft.RemoveOnStateUpListener(TurnRelease, SteamVR_Input_Sources.Any);
        _turnRight.RemoveOnStateDownListener(TurnRightPress, SteamVR_Input_Sources.Any);
        _turnRight.RemoveOnStateUpListener(TurnRelease, SteamVR_Input_Sources.Any);
    }
}
