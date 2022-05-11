using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODAngryVoices : MonoBehaviour
{
    public string officeSceneName;
    public string streetSceneName;
    public float speakChance = 0.3f;

    EventInstance _angryVoicesRWAC;
    EventInstance _angryVoicesRWACNorsk;
    EventInstance _angryVoicesFTM;
    EventInstance _angryVoicesFTMNorsk;
    private int _sceneChoice = 0;
    private float _mood;

    FMOD.Studio.PARAMETER_DESCRIPTION _angryRWACParaDes; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID s_angryRWACParaID; //Gets set as the Parameter ID you will be using.

    FMOD.Studio.PARAMETER_DESCRIPTION _angryFTMParaDes; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID s_angryFTMParaID; //Gets set as the Parameter ID you will be using.

    private void Start()
    {
        if (Random.value > speakChance)
        {
            Destroy(gameObject.GetComponent<FMODAngryVoices>());
        }

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;// Retrieve the name of this scene.
        if (sceneName == (officeSceneName)) { _sceneChoice = 1; } //Change this string to Match the Rebel without a c(l)ause scene
        else if (sceneName == (officeSceneName)) { _sceneChoice = 0; } //Change this string to Match FUCK THE MAN scene.

        Debug.Log("English is " + PlayerPrefs.GetInt("English") + (" & Norwegian is ") + PlayerPrefs.GetInt("Norwegian"));
        if(_sceneChoice == 1)
        {
            if(PlayerPrefs.GetInt("English") == 1 && PlayerPrefs.GetInt("Norwegian") == 0)
            {
                _angryVoicesRWAC = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Angry");
                _angryVoicesRWAC.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(_angryVoicesRWAC, GetComponent<Transform>(), GetComponent<Rigidbody>());
                FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Mood", out _angryRWACParaDes); //Takes out the Parameter you just found by name, through ID.
                s_angryRWACParaID = _angryRWACParaDes.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
            }
            else if (PlayerPrefs.GetInt("English") == 0 && PlayerPrefs.GetInt("Norwegian") == 1)
            {
                _angryVoicesRWACNorsk = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Angry_norsk");
                _angryVoicesRWACNorsk.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(_angryVoicesRWACNorsk, GetComponent<Transform>(), GetComponent<Rigidbody>());
                FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Mood", out _angryRWACParaDes); //Takes out the Parameter you just found by name, through ID.
                s_angryRWACParaID = _angryRWACParaDes.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
            }
        }
        else if (_sceneChoice == 0)
        {
            if(PlayerPrefs.GetInt("English") == 1 && PlayerPrefs.GetInt("Norwegian") == 0)
            {
                _angryVoicesFTM = FMODUnity.RuntimeManager.CreateInstance("event:/FTM/Angry_Complain");
                _angryVoicesFTM.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(_angryVoicesFTM, GetComponent<Transform>(), GetComponent<Rigidbody>());
                FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("FTM_Mood", out _angryFTMParaDes); //Takes out the Parameter you just found by name, through ID.
                s_angryFTMParaID = _angryFTMParaDes.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
            }
            else if (PlayerPrefs.GetInt("English") == 1 && PlayerPrefs.GetInt("Norwegian") == 0)
            {
                _angryVoicesFTMNorsk = FMODUnity.RuntimeManager.CreateInstance("event:/FTM/Angry_Complain_norsk");
                _angryVoicesFTMNorsk.start();
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(_angryVoicesFTMNorsk, GetComponent<Transform>(), GetComponent<Rigidbody>());
                FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("FTM_Mood", out _angryFTMParaDes); //Takes out the Parameter you just found by name, through ID.
                s_angryFTMParaID = _angryFTMParaDes.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
            }

        }
    }

    private void Update()
    {
        CurrentMood();
    }
    private void OnDestroy()
    {
        _angryVoicesRWAC.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Instance so that it does not continue to play after release.
        _angryVoicesRWACNorsk.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_angryRWACParaID, 0); //Resets the playlist when scene is unloaded, remove me if you want the game to remember the last played song on playlist.
        _angryVoicesRWAC.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.
        _angryVoicesRWACNorsk.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.

        _angryVoicesFTM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the  instance so that it does not continue to play after release.
        _angryVoicesFTMNorsk.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the  instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_angryFTMParaID, 0); //Resets the ambience when the scene is unloaded.
        _angryVoicesFTM.release();
        _angryVoicesFTMNorsk.release();
    }
    void CurrentMood()
    {
        _mood = LoDController.s_levelOfDestruction;

        if(_sceneChoice == 1)
        {
            if (_mood > 0.01f)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_angryRWACParaID, 1);
            }
            else if (_mood > 1f)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_angryRWACParaID, 2);
            }
            //else if(_mood < 1f) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_angryRWACParaID, 1); }
        }
        else if(_sceneChoice == 0)
        {
            if (_mood > 0.01f)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_angryFTMParaID, 1);
            }
            else if (_mood > 1f)
            {
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_angryFTMParaID, 2);
            }
            //else if (_mood < 1f) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_angryFTMParaID, 1); }
        }

    }
}
