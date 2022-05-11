using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StartStopVFX : MonoBehaviour
{
    public VisualEffect vfx; //find the proper vfx
    public float playTime = 1.5f; //For various vfx
    
    void Start()
    {
        Invoke("SStop", playTime); //Plays the vfx for 1.5 seconds, (for sparklyspark set to 3)
        Destroy(this.gameObject, 5f); //Destroys it to save space
    }

    private void SStop() //Cause the vfx to stop, is smoother than just destroy
    {
        vfx.Stop();
    }
}
