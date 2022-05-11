using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundOnImpact : MonoBehaviour
{
    public float coolDown = 0.5f;
    public float minimumForce = 2.0f;

    public string sFXSound; //This string is a direct path to the FMOD even, you need to copy its path from FMOD to be certain you get it.
    public GameObject dustBust;

    private float _lastSound;

    private bool CheckShouldPlaySound(Collision collision)
    {
        // For performance, compare the square of impulse to square of breaking impulse
        return collision.impulse.sqrMagnitude > minimumForce * minimumForce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckShouldPlaySound(collision) && Time.time > _lastSound + coolDown)
        {
            _lastSound = Time.time;

            Vector3 contactPoint = collision.GetContact(0).point;

            FMODUnity.RuntimeManager.PlayOneShot(sFXSound, contactPoint);

            if (dustBust != null)
            {
                Instantiate(dustBust, contactPoint, new Quaternion(0, 0, 0, 0));
            }
        }
    }
}
