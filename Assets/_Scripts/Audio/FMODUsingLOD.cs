using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODUsingLOD : MonoBehaviour
{

    public string _officeSceneName;
    public string _streetSceneName;
    public string _cryBabySceneName;
    private float _currentLOD;
    private int _sceneChoice = 0;
    private EventInstance _lODTrackInstanceRWAC;
    private EventInstance _lODTrackInstanceFTM;
    private EventInstance _lODTrackInstanceCB;
    public static bool s_lODOn = false;

    FMOD.Studio.PARAMETER_DESCRIPTION _lODParaDesRWAC; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID _lODParaIDRWAC; //Gets set as the Parameter ID you will be using.

    FMOD.Studio.PARAMETER_DESCRIPTION _lODParaDesFTM; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID _lODParaIDFTM; //Gets set as the Parameter ID you will be using.

    FMOD.Studio.PARAMETER_DESCRIPTION _lODParaDesCB; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID _lODParaIDCB; //Gets set as the Parameter ID you will be using.

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;// Retrieve the name of this scene.
        if (sceneName == (_officeSceneName)) { _sceneChoice = 1; } //Change this string to Match the Rebel without a c(l)ause scene
        else if (sceneName == (_streetSceneName)) { _sceneChoice = 0; } //Change this string to Match FUCK THE MAN scene.
        else if (sceneName == (_cryBabySceneName)) { _sceneChoice = 2; }
        if(_sceneChoice == 1)
        {
            _lODTrackInstanceRWAC = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/LOD");
            _lODTrackInstanceRWAC.start();
            _lODTrackInstanceRWAC.setPaused(true);
            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("LOD", out _lODParaDesRWAC); //Takes out the Parameter you just found by name, through ID.
            _lODParaIDRWAC = _lODParaDesRWAC.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
        }
        else if(_sceneChoice == 0)
        {
            _lODTrackInstanceFTM = FMODUnity.RuntimeManager.CreateInstance("event:/FTM/LOD_song");
            _lODTrackInstanceFTM.start(); //Change my position?
            _lODTrackInstanceFTM.setPaused(false);
            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("LOD_FTM", out _lODParaDesFTM); //Takes out the Parameter you just found by name, through ID.
            _lODParaIDFTM = _lODParaDesFTM.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
        }
        else if(_sceneChoice == 2)
        {
            _lODTrackInstanceCB = FMODUnity.RuntimeManager.CreateInstance("event:/CB/LOD");
            _lODTrackInstanceCB.start(); //Change my position?
            _lODTrackInstanceCB.setPaused(false);
            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("LOD_Store", out _lODParaDesCB); //Takes out the Parameter you just found by name, through ID.
            _lODParaIDCB = _lODParaDesCB.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
        }
    }

    private void FixedUpdate()
    {
        CurrentLOD();
    }

    private void OnDestroy()
    {
        _lODTrackInstanceRWAC.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_lODParaIDRWAC, 0); //Resets the playlist when scene is unloaded, remove me if you want the game to remember the last played song on playlist.
        _lODTrackInstanceRWAC.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.

        _lODTrackInstanceFTM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the  instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_lODParaIDFTM, 0); //Resets the ambience when the scene is unloaded.
        _lODTrackInstanceFTM.release();

        _lODTrackInstanceCB.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the  instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_lODParaIDCB, 0); //Resets the ambience when the scene is unloaded.
        _lODTrackInstanceCB.release();
    }

    void CurrentLOD()
    {
        if(_sceneChoice == 1)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_lODParaIDRWAC, LoDController.s_levelOfDestruction);
            if(LoDController.s_levelOfDestruction >= 0.5f)
            {
                _lODTrackInstanceRWAC.setPaused(false);
                s_lODOn = true;
                FMODPlaylist.s_playListInstanceIpod.setPaused(true);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(FMODPlaylist.s_ambienceParID, 1);
            }
            else if(LoDController.s_levelOfDestruction < 0.5f)
            {
                _lODTrackInstanceRWAC.setPaused(true);
                s_lODOn = false;
                if(FMODPlaylist.s_isPaused == false)
                {
                    FMODPlaylist.s_playListInstanceIpod.setPaused(false);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByID(FMODPlaylist.s_ambienceParID, 0);
                }

            }
        }
        else if (_sceneChoice == 0)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_lODParaIDFTM, _currentLOD);
            if (LoDController.s_levelOfDestruction >= 0.5f)
            {
                _lODTrackInstanceFTM.setPaused(false);
                s_lODOn = true;
                FMODPlaylist.s_playListInstanceMDP.setPaused(true);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(FMODPlaylist.s_ambienceParID, 1);
            }
            else if (LoDController.s_levelOfDestruction < 0.5f)
            {
                _lODTrackInstanceFTM.setPaused(true);
                s_lODOn = false;
                if(FMODPlaylist.s_isPaused == false)
                {
                    FMODPlaylist.s_playListInstanceMDP.setPaused(false);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByID(FMODPlaylist.s_ambienceParID, 0);
                }

            }
        }
        else if (_sceneChoice == 2)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_lODParaIDCB, _currentLOD);
            if (LoDController.s_levelOfDestruction >= 0.5f)
            {
                _lODTrackInstanceCB.setPaused(false);
            }
            else if (LoDController.s_levelOfDestruction < 0.5f)
            {
                _lODTrackInstanceCB.setPaused(true);
            }
        }
    }
}
