using UnityEngine;
public class FMODVolumeControl : MonoBehaviour
{
    #region Variables (Check the slider values)
    FMOD.Studio.Bus _master; //Makes a bus variable, for Master.
    FMOD.Studio.Bus _music;//Makes a bus variable, for Music.
    FMOD.Studio.Bus _sfx; //Makes a bus variable, for SFX.
    FMOD.Studio.Bus _ambience; //Makes a bus variable, for Ambience.
    FMOD.Studio.Bus _voices; //Makes a bus variable, for Voices.

    private float _musicVolume = 0.5f; //IMPORTANT, the slider startup value should be set the same as this value. Is used for the volume slider(for Music). If set to 0.5 then what the bus is set to is its half point. 
    private float _ambienceVolume = 0.5f; //IMPORTANT, the slider startup value should be set the same as this value. Is used for the volume slider(for ambience). If set to 0.5 then what the bus is set to is its half point.
    private float _masterVolume = 1f; //IMPORTANT, the slider startup value should be set the same as this value. Is used for the volume slider (for Master). If set to 1, then max is what the current bus is set to.
    private float _sfxVolume = 0.5f; //IMPORTANT, the slider startup value should be set the same as this value. Is used for the volume slider(for sfx). If set to 0.5 then what the bus is set to is its half point.
    private float _voicesVolume = 0.5f; //IMPORTANT, the slider startup value should be set the same as this value. Is used for the volume slider(for voices). If set to 0.5 then what the bus is set to is its half point.

    #endregion
    #region Awake & Update to find bus and set the SetVolume function.
    void Awake()
    {
        _music = FMODUnity.RuntimeManager.GetBus("bus:/Music"); //Calls upon the bus, so that it can be affected. 
        _ambience = FMODUnity.RuntimeManager.GetBus("bus:/Ambience"); //Calls upon the bus, so that it can be affected.
        _voices = FMODUnity.RuntimeManager.GetBus("bus:/Voice"); //Calls upon the bus, so that it can be affected.
        _sfx = FMODUnity.RuntimeManager.GetBus("bus:/SFX"); //Calls upon the bus, so that it can be affected. 
        _master = FMODUnity.RuntimeManager.GetBus("bus:/"); //Calls upon the bus, so that it can be affected. 

        MasterVolumeLevel(); //_music will set its volume to _musicVolume value. This makes us able to change the value by changing  _musicVolume.
        MusicVolumeLevel(); //_ambience will set its volume to _ambienceVolume value. This makes us able to change the value by changing  _ambienceVolume.
        AmbienceVolumeLevel(); //_sfx will set its volume to _sfxVolume value. This makes us able to change the value by changing  _sfxVolume.
        MusicVolumeLevel(); //_voices will set its volume to _voicesVolume value. This makes us able to change the value by changing  _voicesVolume.
        SFXVolumeLevel(); //_master will set its volume to _masterVolume value. This makes us able to change the value by changing  _masterVolume.

    }
    void Update()
    {
        MasterVolumeLevel(); //_music will set its volume to _musicVolume value. This makes us able to change the value by changing  _musicVolume.
        MusicVolumeLevel(); //_ambience will set its volume to _ambienceVolume value. This makes us able to change the value by changing  _ambienceVolume.
        AmbienceVolumeLevel(); //_sfx will set its volume to _sfxVolume value. This makes us able to change the value by changing  _sfxVolume.
        MusicVolumeLevel(); //_voices will set its volume to _voicesVolume value. This makes us able to change the value by changing  _voicesVolume.
        SFXVolumeLevel(); //_master will set its volume to _masterVolume value. This makes us able to change the value by changing  _masterVolume.
    }
    #endregion
    #region Volume Level Voids
    public void MasterVolumeLevel () //Function to change the volume through a slider. 
    {
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume"); //Sets the volume to the new float value from the slider value.
        _master.setVolume(_masterVolume);
    }
    
    public void MusicVolumeLevel () //Function to change the volume through a slider. 
    {
        _musicVolume = PlayerPrefs.GetFloat("MusicVolume"); //Sets the volume to the new float value from the slider value.
        _music.setVolume(_musicVolume);
    }

    public void AmbienceVolumeLevel () //Function to change the volume through a slider. 
    {
        _ambienceVolume = PlayerPrefs.GetFloat("AmbienceVolume"); //Sets the volume to the new float value from the slider value.
        _ambience.setVolume(_ambienceVolume);
    }

    public void SFXVolumeLevel() //Function to change the volume through a slider. 
    {
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume"); //Sets the volume to the new float value from the slider value.
        _sfx.setVolume(_sfxVolume);
    }

    public void VoicesVolumeLevel() //Function to change the volume through a slider. 
    {
        _voicesVolume = PlayerPrefs.GetFloat("VoicesVolume"); //Sets the volume to the new float value from the slider value.
        _voices.setVolume(_voicesVolume);
    }
    #endregion
}
