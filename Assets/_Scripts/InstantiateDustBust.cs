using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InstantiateDustBust : MonoBehaviour
{
    public GameObject dustBust;
    
    private void OnDisable()
    {
        Instantiate(dustBust, this.transform.position, Quaternion.identity); //instantiates the vfx at the broken objects position
    }
}
