using FMOD.Studio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODPlaylist : MonoBehaviour
{
    #region Variables

    public string _officeSceneName;
    public string _streetSceneName;
    public string _CryBabySceneName;
    public static int s_scenechoice = 0;
    #region RWAC
    //[FMODUnity.EventRef] [SerializeField] string _playList; //Refrence to the FMOD Event.
    public static EventInstance s_playListInstanceIpod; //An instance for the playlist.
    public static int s_playListPlaceIpod = 0; //The current placement in the music list.
    public static int s_playListSizeIpod = 9; //Change this to match the amount of songs in the playlist. (Only works for a playlist with 1 local parameter.)
    public static bool s_isPaused; //Is the song paused?

    FMOD.Studio.PARAMETER_DESCRIPTION _parameterDescriptionIpod; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID _parameterIDIpod; //Gets set as the Parameter ID you will be using.

    //[FMODUnity.EventRef] [SerializeField] string _ambience; //Rerfrence to FMOD event of ambience.
    public static EventInstance s_ambienceInstance; //An instance for the ambince.
    FMOD.Studio.PARAMETER_DESCRIPTION _ambienceParaDescription; //Used to find the ID of the parameter.
    public static FMOD.Studio.PARAMETER_ID s_ambienceParID; //Is the ID of the parameter we want to use.
    #endregion
    #region FTM
    public static EventInstance s_playListInstanceMDP; //An instance for the playlist.
    public static int s_playListPlaceMDP = 0; //The current placement in the music list.
    public static int s_playListSizeMDP = 14; //Change this to match the amount of songs in the playlist. (Only works for a playlist with 1 local parameter.)

    FMOD.Studio.PARAMETER_DESCRIPTION _parameterDescriptionMDP; //Used to get out the Parameter ID you are planning on using.
    static FMOD.Studio.PARAMETER_ID _parameterIDMDP; //Gets set as the Parameter ID you will be using.

    public static EventInstance s_citySoundInstsance; //An instance for the city sound.
    public static EventInstance s_citySound; //An instance for the city sound.
    #endregion
    #region CB
    EventInstance _ambienceCBInstance;
    EventInstance _backgroundCBInstance;
    #endregion
    #endregion
    #region Start & Update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;// Retrieve the name of this scene.
        if(sceneName == (_officeSceneName)) { s_scenechoice = 1; } //Change this string to Match the Rebel without a c(l)ause scene
        else if (sceneName == (_streetSceneName)) { s_scenechoice = 0; } //Change this string to Match FUCK THE MAN scene.
        else if (sceneName == (_CryBabySceneName)) { s_scenechoice = 2; }
        if (s_scenechoice == 1)
        {
            #region RWAC
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Announcer_RebelWithoutAClause");
            s_playListInstanceIpod = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Ipod"); //Lets the event know which song to play.
            s_playListInstanceIpod.start(); //Starts the Playlist at start.

            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Ipod Controls", out _parameterDescriptionIpod); //Takes out the Parameter you just found by name, through ID.
            _parameterIDIpod = _parameterDescriptionIpod.id; //Sets the ID found above as the Parameter ID for use in the rest of the script

            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Ambient", out _ambienceParaDescription); //Takes out the Parameter you just found by name, through ID.
            s_ambienceParID = _ambienceParaDescription.id; //Sets the ID found above as the Parameter ID for use in the rest of the script

            s_ambienceInstance = FMODUnity.RuntimeManager.CreateInstance("event:/RWAC/Ambient"); //Lets the event know which song to play.
            s_ambienceInstance.start();
            #endregion
        }
        else if (s_scenechoice == 0)
        {
            #region FTM
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Announcer_FuckTheMan");
            s_playListInstanceMDP = FMODUnity.RuntimeManager.CreateInstance("event:/FTM/Mini_disc"); //Lets the event know which song to play.
            s_playListInstanceMDP.start(); //Starts the Playlist at start.

            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Mini disc", out _parameterDescriptionMDP); //Takes out the Parameter you just found by name, through ID.
            _parameterIDMDP = _parameterDescriptionMDP.id; //Sets the ID found above as the Parameter ID for use in the rest of the script

            FMODUnity.RuntimeManager.StudioSystem.getParameterDescriptionByName("Ambient", out _ambienceParaDescription); //Takes out the Parameter you just found by name, through ID.
            s_ambienceParID = _ambienceParaDescription.id; //Sets the ID found above as the Parameter ID for use in the rest of the script

            s_citySoundInstsance = FMODUnity.RuntimeManager.CreateInstance("event:/FTM/City_sound"); //Lets the event know which song to play.
            s_citySoundInstsance.start();
            #endregion
        }
        else if(s_scenechoice == 2)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Announcer_CryBaby");
            _ambienceCBInstance = FMODUnity.RuntimeManager.CreateInstance("event:/CB/Ambient music");
            _backgroundCBInstance = FMODUnity.RuntimeManager.CreateInstance("event:/CB/Background_noise");
            _ambienceCBInstance.start();
            _backgroundCBInstance.start();
        }
        Debug.Log("current scene is " + s_scenechoice);

    }
    #region What if Destroyed
    private void OnDestroy()
    {
        if (s_scenechoice == 1)
        {
            #region RWAC
            s_playListInstanceIpod.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the PlayListInstance so that it does not continue to play after release.
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDIpod, 0); //Resets the playlist when scene is unloaded, remove me if you want the game to remember the last played song on playlist.
            s_playListInstanceIpod.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.

            s_ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Ambienve instance so that it does not continue to play after release.
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); //Resets the ambience when the scene is unloaded.
            s_ambienceInstance.release();
            #endregion
        }
        else if (s_scenechoice == 0)
        {
            #region FTM
            s_playListInstanceMDP.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the Instance so that it does not continue to play after release.
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDMDP, 0); //Resets the playlist when scene is unloaded, remove me if you want the game to remember the last played song on playlist.
            s_playListInstanceMDP.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.

            s_citySoundInstsance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the city sound instance so that it does not continue to play after release.
            FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); //Resets the ambience when the scene is unloaded.
            s_citySoundInstsance.release();
            #endregion
        }
        else if (s_scenechoice == 2)
        {
            #region CB
            _ambienceCBInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //Stops the PlayListInstance so that it does not continue to play after release.
            _backgroundCBInstance.release(); //Releases the instance(removes) at the end of the objects lifespan, makes the game not load multiple instance.
            #endregion
        }
    }
    #endregion
    private void Update()
    {
        SongStopped();
    }
    #endregion
    #region Last song / Next song function
    public static void LastNext(bool next)
    {
        if (s_scenechoice == 1)
        {
            #region RWAC
            if (next && FMODUsingLOD.s_lODOn == false) //Condition to go to next song(Parameter value)
            {
                s_playListPlaceIpod += 1; //Increases the placement the script knows about.
                if (s_playListPlaceIpod == s_playListSizeIpod) { s_playListPlaceIpod = 0; } //Stops the Placement from going out of bounds.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDIpod, s_playListPlaceIpod); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceIpod.start(); //Restarts song
                s_playListInstanceIpod.setPaused(false); //Unpauses event.
                s_isPaused = false; // Updates the bool.
                if (s_playListPlaceIpod != 0) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); } //If the Ipod is playing a song then you can not hear the ambience as well.
                else { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); } //If the Ipod is not playing any song (0) then the ambience can be heard.
            }
            else if (!next && FMODUsingLOD.s_lODOn == false) //Condition to go to last song(Parameter value)
            {
                s_playListPlaceIpod -= 1; //decreases the placement the script knows about.
                if (s_playListPlaceIpod == -1) { s_playListPlaceIpod = s_playListSizeIpod - 1; } //Stops the Placement from going out of bounds.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDIpod, s_playListPlaceIpod); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceIpod.start(); //Restarts song
                s_playListInstanceIpod.setPaused(false); //Unpauses event.
                s_isPaused = false; // Updates the bool.
                if (s_playListPlaceIpod != 0) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); }//If the Ipod is playing a song then you can not hear the ambience as well.
                else { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); }//If the Ipod is not playing any song (0) then the ambience can be heard.
            }
            #endregion
        }
        else if (s_scenechoice == 0)
        {
            #region FTM
            if (next && FMODUsingLOD.s_lODOn == false) //Condition to go to next song(Parameter value)
            {
                s_playListPlaceMDP += 1; //Increases the placement the script knows about.
                if (s_playListPlaceMDP == s_playListSizeMDP) { s_playListPlaceMDP = 0; } //Stops the Placement from going out of bounds.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDMDP, s_playListPlaceMDP); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceMDP.start(); //Restarts song
                s_playListInstanceMDP.setPaused(false); //Unpauses event.
                s_isPaused = false; // Updates the bool.
                if (s_playListPlaceMDP != 0) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); } //If the Ipod is playing a song then you can not hear the ambience as well.
                else { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); } //If the Ipod is not playing any song (0) then the ambience can be heard.
                Debug.Log("Current track is" + s_playListPlaceMDP);

            }
            else if (!next && FMODUsingLOD.s_lODOn == false) //Condition to go to last song(Parameter value)
            {
                s_playListPlaceMDP -= 1; //decreases the placement the script knows about.
                if (s_playListPlaceMDP == -1) { s_playListPlaceMDP = s_playListSizeMDP - 1; } //Stops the Placement from going out of bounds.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDMDP, s_playListPlaceMDP); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceMDP.start(); //Restarts song
                s_playListInstanceMDP.setPaused(false); //Unpauses event.
                s_isPaused = false; // Updates the bool.
                if (s_playListPlaceMDP != 0) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); }//If the Ipod is playing a song then you can not hear the ambience as well.
                else { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); }//If the Ipod is not playing any song (0) then the ambience can be heard.
            }
            #endregion
        }
    }
    #endregion
    #region Pause / Play function
    public static void PausePlay()
    {
        if (s_scenechoice == 1)
        {
            #region RWAC
            if (!s_isPaused && s_playListPlaceIpod != 0 && FMODUsingLOD.s_lODOn == false) //If P is pressed & the _isPaused bool is false, then the function will go through to pause.
            {
                s_playListInstanceIpod.setPaused(true); //sets it to paused
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); //If the Ipod is paused the ambience can be better heard.
                s_isPaused = true; // Updates the bool.
            }
            else if (s_isPaused && s_playListPlaceIpod != 0 && FMODUsingLOD.s_lODOn == false) // If P is pressed & the _isPaused bool is true, then the function will go through to unpause.
            {
                s_playListInstanceIpod.setPaused(false); //Unpauses event.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); //If the Ipod is playing again the ambience is muffled.
                s_isPaused = false; // Updates the bool.
            }
            else if (s_playListPlaceIpod == 0 && FMODUsingLOD.s_lODOn == false)
            {
                s_playListPlaceIpod += 1; //Increases the placement the script knows about.
                if (s_playListPlaceIpod == s_playListSizeIpod) { s_playListPlaceIpod = 0; } //Stops the Placement from going out of bounds.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDIpod, s_playListPlaceIpod); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceIpod.start(); //Restarts song
                s_playListInstanceIpod.setPaused(false); //Unpauses event.
                s_isPaused = false; // Updates the bool.
                if (s_playListPlaceIpod != 0) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); } //If the Ipod is playing a song then you can not hear the ambience as well.
                else { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); } //If the Ipod is not playing any song (0) then the ambience can be heard.
                Debug.Log("Start Music");
            }
            #endregion
        }
        else if (s_scenechoice == 0)
        {
            #region FTM
            if (!s_isPaused && s_playListPlaceMDP != 0 && FMODUsingLOD.s_lODOn == false) //If P is pressed & the _isPaused bool is false, then the function will go through to pause.
            {
                s_playListInstanceMDP.setPaused(true); //sets it to paused
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); //If the Ipod is paused the ambience can be better heard.
                s_isPaused = true; // Updates the bool.
            }
            else if (s_isPaused && s_playListPlaceMDP != 0 && FMODUsingLOD.s_lODOn == false) // If P is pressed & the _isPaused bool is true, then the function will go through to unpause.
            {
                s_playListInstanceMDP.setPaused(false); //Unpauses event.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); //If the Ipod is playing again the ambience is muffled.
                s_isPaused = false; // Updates the bool.
            }
            else if (s_playListPlaceMDP == 0 && FMODUsingLOD.s_lODOn == false)
            {
                s_playListPlaceIpod += 1; //Increases the placement the script knows about.
                if (s_playListPlaceMDP == s_playListSizeMDP) { s_playListPlaceMDP = 0; } //Stops the Placement from going out of bounds.
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDMDP, s_playListPlaceMDP); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceMDP.start(); //Restarts song
                s_playListInstanceMDP.setPaused(false); //Unpauses event.
                s_isPaused = false; // Updates the bool.
                if (s_playListPlaceMDP != 0) { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 1); } //If the Ipod is playing a song then you can not hear the ambience as well.
                else { FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); } //If the Ipod is not playing any song (0) then the ambience can be heard.
            }
            #endregion
        }
    }
    #endregion
    #region Song has Stopped function
    void SongStopped()
    {
        if (s_scenechoice == 0)
        {
            #region RWAC
            PLAYBACK_STATE _playbackStateIpod;
            s_playListInstanceIpod.getPlaybackState(out _playbackStateIpod);
            if (_playbackStateIpod == PLAYBACK_STATE.SUSTAINING)
            {
                s_playListInstanceIpod.triggerCue(); //Triggers a cue that lets it play beyound the sustain.
                Debug.Log("Song Stopped");
                s_playListPlaceIpod += 1; //Increases the placement the script knows about.
                if (s_playListPlaceIpod == s_playListSizeIpod)
                {
                    s_playListPlaceIpod = 0;//Stops the Placement from going out of bounds.
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); //If the playlist went to the end, then it will stop and give ambience once more.
                }
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDIpod, s_playListPlaceIpod); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceIpod.start(); //Restarts song
                Debug.Log("Current song is " + s_playListPlaceIpod);
            }
            #endregion
        }
        else if (s_scenechoice == 1)
        {
            #region FTM
            PLAYBACK_STATE _playbackStateMDP;
            s_playListInstanceMDP.getPlaybackState(out _playbackStateMDP);
            if (_playbackStateMDP == PLAYBACK_STATE.SUSTAINING)
            {
                s_playListInstanceMDP.triggerCue(); //Triggers a cue that lets it play beyound the sustain.
                Debug.Log("Song Stopped");
                s_playListPlaceMDP += 1; //Increases the placement the script knows about.
                if (s_playListPlaceMDP == s_playListSizeMDP)
                {
                    s_playListPlaceMDP = 0;//Stops the Placement from going out of bounds.
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByID(s_ambienceParID, 0); //If the playlist went to the end, then it will stop and give ambience once more.
                }
                FMODUnity.RuntimeManager.StudioSystem.setParameterByID(_parameterIDMDP, s_playListPlaceMDP); //Sets the parameter to match the wanted song through placement.
                s_playListInstanceMDP.start(); //Restarts song
                Debug.Log("Current song is " + s_playListPlaceMDP);
            }
            #endregion
        }
    }
    #endregion
}