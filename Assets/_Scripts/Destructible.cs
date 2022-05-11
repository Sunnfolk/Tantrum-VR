using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Destructible : MonoBehaviour
{
    [SerializeField] private float _lodPoints = 0.2f;
    [Space(5)]
    [Tooltip("The minimum momentum to break the object")]
    public float breakingImpulse = 5.0f;
    public List<GameObject> destroyedObjectVersions;
    public string sFXSound; //This string is a direct path to the FMOD even, you need to copy its path from FMOD to be certain you get it.
    public GameObject dustBust;

    public delegate void OnDestroyed(GameObject newObject);
    public event OnDestroyed onDestroyed;

    private Rigidbody _rigidbody;
    private bool _hasBeenBroken;

    private void Start()
    { 
        _rigidbody = GetComponent<Rigidbody>();
    }

    private bool CheckShouldBeDestroyed(Collision collision) {
        // For performance, compare the square of impulse to square of breaking impulse
        return collision.impulse.sqrMagnitude > breakingImpulse * breakingImpulse;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasBeenBroken) return;

        if (CheckShouldBeDestroyed(collision))
        {
            _hasBeenBroken = true;

            Vector3 contactPoint = collision.GetContact(0).point;

            //spawn a randomly selected destroyed version of the object
            GameObject destroyedVersion = Instantiate(destroyedObjectVersions[Random.Range(0, destroyedObjectVersions.Count)], transform.position, transform.rotation);
            destroyedVersion.transform.localScale = transform.lossyScale;

            //get the rigidbodies on the broken pieces
            Rigidbody[] pieceRigidbodies = destroyedVersion.GetComponentsInChildren<Rigidbody>();

            //set the velocity of all the broken pieces to the velocity of this object
            foreach (Rigidbody rigidbody in pieceRigidbodies)
            {
                rigidbody.mass = _rigidbody.mass / pieceRigidbodies.Length;

                rigidbody.velocity = _rigidbody.velocity;
                if (collision.rigidbody != null) _rigidbody.velocity += collision.rigidbody.velocity * collision.rigidbody.mass;
            }
            //Debug.Log((-collision.relativeVelocity));
            
            FMODUnity.RuntimeManager.PlayOneShot(sFXSound, contactPoint);

            if(dustBust != null)
            {
                Instantiate(dustBust, contactPoint, new Quaternion(0, 0, 0, 0));
            }
            

            //add Level of destruction
            LoDController.s_levelOfDestruction += _lodPoints;

            //play sound with fmod somewhere here
            onDestroyed?.Invoke(destroyedVersion);
            Destroy(this.gameObject);
        }
    }
}
