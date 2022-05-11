using FMOD.Studio;
using UnityEngine;

public class FMODCommentsBC : MonoBehaviour
{

    EventInstance _commentsCB;
    EventInstance _commentsCBNorsk;
    public float speakChance = 0.3f;
    private float _mood;

    FMOD.Studio.PARAMETER_DESCRIPTION _commentsCBaraDes; //Used to get out the Parameter ID you are planning on using.
    FMOD.Studio.PARAMETER_ID _commentCBaraID; //Gets set as the Parameter ID you will be using.

    private void Start()
    {
        if (Random.value > speakChance)
        {
            Destroy(gameObject.GetComponent<FMODAngryVoices>());
        }

        if (PlayerPrefs.GetInt("English") == 1 && PlayerPrefs.GetInt("Norwegian") == 0)
        {
            _commentsCB = FMODUnity.RuntimeManager.CreateInstance("event:/CB/Comments");
            _commentsCB.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(_commentsCB, GetComponent<Transform>(), GetComponent<Rigidbody>());
            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Mood_Store", out _commentsCBaraDes); //Takes out the Parameter you just found by name, through ID.
            _commentCBaraID = _commentsCBaraDes.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
        }
        else if (PlayerPrefs.GetInt("English") == 0 && PlayerPrefs.GetInt("Norwegian") == 1)
        {
            _commentsCBNorsk = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Angry_norsk");
            _commentsCBNorsk.start();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(_commentsCBNorsk, GetComponent<Transform>(), GetComponent<Rigidbody>());
            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Mood", out _commentsCBaraDes); //Takes out the Parameter you just found by name, through ID.
            _commentCBaraID = _commentsCBaraDes.id; //Sets the ID found above as the Parameter ID for use in the rest of the script
        }
    }

    private void OnDestroy()
    {
        _commentsCB.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_commentCBaraID, 0); //Resets the playlist when scene is unloaded, remove me if you want the game to remember the last played song on playlist.
        _commentsCB.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.

        _commentsCBNorsk.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the  instance so that it does not continue to play after release.
        FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_commentCBaraID, 0); //Resets the ambience when the scene is unloaded.
        _commentsCBNorsk.release();
    }

    private void Update()
    {
        _mood = LoDController.s_levelOfDestruction;


        if (_mood > 0.01f)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_commentCBaraID, 1);
        }
        else if (_mood > 1f)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_commentCBaraID, 2);
        }
    }
}
