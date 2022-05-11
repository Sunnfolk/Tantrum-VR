using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODWindowBreak : MonoBehaviour
{
    private int _windowBreak;

    FMOD.Studio.PARAMETER_DESCRIPTION _parameterDescriptionWindow; //Used to get out the Parameter ID you are planning on using.
    FMOD.Studio.PARAMETER_ID _parameterIDWindow; //Gets set as the Parameter ID you will be using.

    private void Start()
    {
        _windowBreak = 0;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDWindow, _windowBreak);
        FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Window", out _parameterDescriptionWindow); //Takes out the Parameter you just found by name, through ID.
        _parameterIDWindow = _parameterDescriptionWindow.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
    }
    private void OnDestroy()
    {
        _windowBreak += 1;
        _windowBreak = Mathf.Clamp(_windowBreak, 0, 2);
        if(_windowBreak == 1)
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDWindow, _windowBreak);

    }


}
