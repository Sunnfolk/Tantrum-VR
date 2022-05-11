using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private Transform _teleportPoint;
    [SerializeField] private Transform _headset;
    [SerializeField] private Transform[] _controllers;
    [SerializeField] private Vector3 _teleportPointOffset;
    private SteamVR_Input_Sources[] _inputs;
    private int _currentController;
    [Space(5)]
    [SerializeField] private float _arcDropOff;
    [SerializeField] private float _segmentSize;
    [SerializeField] private int _maxSegments;
    [Tooltip("layers that can be teleported to")]
    [SerializeField] private LayerMask _teleportLayer;
    [Tooltip("layers that block the teleport arc")]
    [SerializeField] private LayerMask _stopTeleportArcLayer;
    [Space(10)]
    [SerializeField] private SteamVR_Action_Boolean _teleportInput;
    [Space(5)]
    [SerializeField] private LineRenderer _line;

    private bool _createArc;
    private bool _canTeleport;


    void Start()
    {
        //Debug.Log("we,re runn..teleporting");
        //add listeners to all controller inputs to be able to differenciate which controller is being used
        _inputs = new SteamVR_Input_Sources[_controllers.Length];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _controllers[i].gameObject.GetComponentInParent<SteamVR_Behaviour_Pose>().inputSource;

            _teleportInput.AddOnStateDownListener(TeleportPress, _inputs[i]);
            _teleportInput.AddOnStateUpListener(TeleportRelease, _inputs[i]);
        }
    }


    void Update()
    {
        if (_createArc)
        {
            //start of by setting canteleport to false, so it doesn't get affected by the previous frame
            _canTeleport = false;

            //set the arc starting point
            Vector3 currentPos = _controllers[_currentController].position;
            Vector3 currentDir = _controllers[_currentController].forward;

            //contains all the positions for the linerenderer to draw the arc
            Vector3[] arcPoints = new Vector3[_maxSegments+2];
            int totalPoints = _maxSegments;

            for (int i = 0; i < _maxSegments; i++)
            {
                //do a raycast from a little behind the current position in the arc to chack for objects blocking
                //the slight offset is because if a raycast starts within a collider it will not hit that collider
                RaycastHit arcRaycast;
                Physics.Raycast(currentPos - (currentDir * 0.05f), currentDir, out arcRaycast, _segmentSize + 0.05f, (_stopTeleportArcLayer | _teleportLayer));

                //save the current point and set the next point
                arcPoints[i] = currentPos;
                currentPos += currentDir * _segmentSize;

                //decrement the y direction
                currentDir.y -= _arcDropOff;

                if (arcRaycast.collider != null)
                {
                    //set the final arc point if the raycast hit something and save the total amount of points
                    arcPoints[i+1] = arcRaycast.point;
                    totalPoints = i+2;

                    //set canteleport to true if the object the raycast hit is on a layer that is included in the _teleportLayer layermask
                    _canTeleport = _teleportLayer == (_teleportLayer | (1 << arcRaycast.transform.gameObject.layer));
                    break;              
                }
            }

            //set the teleport point visual indication active if the player can teleport
            _teleportPoint.gameObject.SetActive(_canTeleport);
            //set the position to the last position on the arc
            _teleportPoint.position = arcPoints[totalPoints - 1];

            //set the line renderer to display arc
            _line.positionCount = totalPoints;
            _line.SetPositions(arcPoints);
        }
    }

    void TeleportPress(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromInput)
    {
        if (!MenuInputManager.s_deviceIsUp)
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                if (fromInput == _inputs[i])
                {
                    //set createArc to true and set current ceontroller to whatever the input came from
                    _currentController = i;
                    _createArc = true;
                    break;
                }
            }
        }
    }

    void TeleportRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromInput)
    {
        if (fromInput == _inputs[_currentController] && !MenuInputManager.s_deviceIsUp)
        {
            //set the player position to the teleport point if the input matches the current input and canTeleport is true
            if (_canTeleport)
            {
                _teleportPoint.gameObject.SetActive(false);
                Vector3 worldHeadsetOffset = transform.TransformVector(_headset.localPosition);
                transform.position = _teleportPoint.position - new Vector3(worldHeadsetOffset.x, 0, worldHeadsetOffset.z);
            }
            //don't make arc
            _createArc = false;
            _line.positionCount = 0;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _inputs.Length; i++)
        {
            _teleportInput.RemoveOnStateDownListener(TeleportPress, _inputs[i]);
            _teleportInput.RemoveOnStateUpListener(TeleportRelease, _inputs[i]);
        }
    }
}
