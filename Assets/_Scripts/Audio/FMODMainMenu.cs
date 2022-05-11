using FMOD.Studio;
using UnityEngine;

public class FMODMainMenu : MonoBehaviour
{
    public string mainMenu; //This string is a direct path to the FMOD even, you need to copy its path from FMOD to be certain you get it.
    EventInstance _mainMenuInstance;

    EventInstance _tantrumVR;

    private void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Announcer_TantrumVR");

        _mainMenuInstance = FMODUnity.RuntimeManager.CreateInstance(mainMenu);
        _mainMenuInstance.start();
    }
    private void OnDestroy()
    {
        _mainMenuInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _mainMenuInstance.release();
    }

}
