using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODRBackground : MonoBehaviour
{
    public string _officeSceneName;
    public string _streetSceneName;
    public string _cryBabySceneName;
    private int _sceneChoice;

    private EventInstance _backgroundInstance;
    private EventInstance _outdoorInstance;
    private EventInstance _backgroundInstanceNorsk;



    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;// Retrieve the name of this scene.
        if (sceneName == (_officeSceneName)) { _sceneChoice = 1; } //Change this string to Match the Rebel without a c(l)ause scene
        else if (sceneName == (_streetSceneName)) { _sceneChoice = 0; } //Change this string to Match FUCK THE MAN scene.
        else if (sceneName == (_cryBabySceneName)) { _sceneChoice = 2; }

        if(_sceneChoice == 1)
        {
           if(PlayerPrefs.GetInt("English") == 1 && PlayerPrefs.GetInt("Norwegian") == 0)
            {
                _backgroundInstance = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Background_noise"); //Lets the event know which song to play.
                _backgroundInstance.start(); //Starts the Playlist at start.
            }
           else if(PlayerPrefs.GetInt("English") == 0 && PlayerPrefs.GetInt("Norwegian") == 1)
            {
                _backgroundInstanceNorsk = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Background_noise_norsk"); //Lets the event know which song to play.
                _backgroundInstanceNorsk.start(); //Starts the Playlist at start.
            }
            _outdoorInstance = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Outdoor_noise"); //Lets the event know which song to play.
            _outdoorInstance.start(); //Starts the Playlist at start.
        }
        else if (_sceneChoice == 2)  //Does nothing yet, here for the store.
        {

        }

    }
    private void OnDestroy()
    {
        _backgroundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Instance so that it does not continue to play after release.
        _backgroundInstance.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.

        _outdoorInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the instance so that it does not continue to play after release.
        _outdoorInstance.release();

        _backgroundInstanceNorsk.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the instance so that it does not continue to play after release.
        _backgroundInstanceNorsk.release();
    }

}
