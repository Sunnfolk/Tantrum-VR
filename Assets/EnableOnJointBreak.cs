using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnJointBreak : MonoBehaviour
{
    public GameObject objectToEnable;

    private void OnJointBreak(float breakForce)
    {
        objectToEnable.SetActive(true);
    }
}
