using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class UI_LaserPointer : MonoBehaviour
{
    [SerializeField] private Transform[] _controllers;
    [SerializeField] private SteamVR_Action_Boolean _input;
    [Space(10)]
    [SerializeField] private Button _emptyButton;

    private SteamVR_Input_Sources[] _inputSources;
    private LineRenderer _laser;
    private Button _currentButton;
    private Button _prevButton;

    private int _currentController = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _laser.positionCount = 2;

        _inputSources = new SteamVR_Input_Sources[_controllers.Length];

        for (int i = 0; i < _controllers.Length; i++)
        {
            _inputSources[i] = _controllers[i].GetComponent<SteamVR_Behaviour_Pose>().inputSource;

            _input.AddOnStateDownListener(OnClick, _inputSources[i]);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _inputSources.Length; i++)
        {
            _input.RemoveOnStateDownListener(OnClick, _inputSources[i]);
        }
    }

    void FixedUpdate()
    {
        RaycastHit raycastHit;
        Physics.Raycast(_controllers[_currentController].position, _controllers[_currentController].forward, out raycastHit, 10);

        Vector3 endPoint;
        if (raycastHit.transform != null)
        {
            endPoint = raycastHit.point;
            _currentButton = raycastHit.transform.GetComponent<Button>();
        }
        else
        {
            endPoint = _controllers[_currentController].position + (_controllers[_currentController].forward * 10);
            _currentButton = null;
        }
        
        _laser.SetPositions(new Vector3[] { _controllers[_currentController].position, endPoint });


        if(_currentButton != _prevButton && _currentButton != null)
        {
            _currentButton.Select();
            _prevButton = _currentButton;
        }
        else if(_currentButton != _prevButton && _currentButton == null)
        {
            _emptyButton.Select();
            _prevButton = _currentButton;
        }
    }

    void OnClick(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(fromSource == _inputSources[_currentController])
        {
            if(_currentButton != null)
            {
                _currentButton.onClick.Invoke();
            }
        }
        else
        {
            for (int i = 0; i < _inputSources.Length; i++)
            {
                if(_inputSources[i] == fromSource)
                {
                    _currentController = i;
                }
            }
        }
    }
}
