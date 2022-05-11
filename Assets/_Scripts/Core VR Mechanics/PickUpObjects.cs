using UnityEngine;
using Valve.VR;

public class PickUpObjects : MonoBehaviour
{
    private SteamVR_Behaviour_Pose _trackedObj;
    private Rigidbody _rb;
    [SerializeField] private ConfigurableJoint _joint;

    private SteamVR_Input_Sources _inputSource;
    [SerializeField] private SteamVR_Action_Boolean _grabInput;
    [Space(3)]
    [SerializeField] private GameObject _handModel;
    [Space(10)]
    [Header("Configurable joint configuration")]
    [SerializeField] private float _distanceFromHandLimit;
    [SerializeField] private float _rotationFromHandleLimit;
    [Space(3)]
    [SerializeField] private float _positionDriveSpring;
    [SerializeField] private float _positionDriveMaxForce;
    [Space(3)]
    [SerializeField] private float _rotationDriveSpring;
    [SerializeField] private float _rotationDriveMaxForce;
    [Space(10)]
    [Tooltip("layers that can be grabbed by player")]
    [SerializeField] private LayerMask _grabbableLayers;

    //join configuration variables
    private SoftJointLimit _linearLimit;
    private SoftJointLimit _rotationalLimit;
    private JointDrive _positionDrive;
    private JointDrive _rotationDrive;

    void Start()
    {
        //put all the inspector input joint configurations into the joint variables
        _linearLimit.limit = _distanceFromHandLimit;
        _rotationalLimit.limit = _rotationFromHandleLimit;
        _positionDrive.positionSpring = _positionDriveSpring;
        _positionDrive.maximumForce = _positionDriveMaxForce;
        _rotationDrive.positionSpring = _rotationDriveSpring;
        _rotationDrive.maximumForce = _rotationDriveMaxForce;

        _rb = GetComponent<Rigidbody>();

        //get input source and tracked object from the controller object parrent
        _trackedObj = GetComponentInParent<SteamVR_Behaviour_Pose>();
        _inputSource = GetComponentInParent<SteamVR_Behaviour_Pose>().inputSource;

        //assign functions that are called when the input action goes from false to true and true to false
        _grabInput.AddOnStateDownListener(GripGrab, _inputSource); 
        _grabInput.AddOnStateUpListener(GripRelease, _inputSource);
    }

    private void OnDestroy()
    {
        _grabInput.RemoveOnStateDownListener(GripGrab, _inputSource);
        _grabInput.RemoveOnStateUpListener(GripRelease, _inputSource);
    }

    //do the grabby magic
    void GripGrab(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (_joint != null && _joint.connectedBody != _rb)
        {
            _joint = null;
        }

        if (_joint != null)
        {
            return;
        }

        //get all colliders within the garb area on the gabbable layers
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.lossyScale.x / 2, _grabbableLayers);

        float shortestDistance = 100; //set the shortest distance to something stupidly high

        GameObject go = null; //grabbed object


        //check which collider is closest to the center of the grab area and save it
        foreach (Collider collider in hitColliders)
        {
            Vector3 closestPoint = collider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);

            if (distance < shortestDistance)
            {
                go = collider.attachedRigidbody.gameObject;

                if (go != null)
                {
                    shortestDistance = distance;
                }
            }
        }

        if (go != null)
        {

            if (go.CompareTag("FixedPivot") || go.CompareTag("lockedInHand"))
            {
                //set the object position and rotation to the grab areas position/rotation if the object has a fixed pivot
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;

                FireArm fa = go.GetComponent<FireArm>();
                if (fa != null)
                {
                    //set the firearm to be able to fire
                    fa.CanFire(true, _inputSource);
                }
            }

            //get the config joint so that if it's already picked up and has one it just swapps hands
            _joint = go.GetComponent<ConfigurableJoint>();

            if(_joint == null)
            {
                //add a config joint and assign it all the values we need
                _joint = go.AddComponent<ConfigurableJoint>();

                _joint.linearLimit = _linearLimit;
                _joint.highAngularXLimit = _rotationalLimit;
                _joint.lowAngularXLimit = _rotationalLimit;
                _joint.angularYLimit = _rotationalLimit;
                _joint.angularZLimit = _rotationalLimit;

                _joint.xMotion = ConfigurableJointMotion.Limited;
                _joint.yMotion = ConfigurableJointMotion.Limited;
                _joint.zMotion = ConfigurableJointMotion.Limited;
                _joint.angularXMotion = ConfigurableJointMotion.Limited;
                _joint.angularYMotion = ConfigurableJointMotion.Limited;
                _joint.angularZMotion = ConfigurableJointMotion.Limited;

                _joint.xDrive = _positionDrive;
                _joint.yDrive = _positionDrive;
                _joint.zDrive = _positionDrive;

                _joint.angularXDrive = _rotationDrive;
                _joint.angularYZDrive = _rotationDrive;

                //set the collision detetion to continous dynamic fo better collision detection
                _joint.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
   
            //set the parrent to the controller, this way the held objects teleport with the player
            go.transform.parent = transform;

            //set the joints connected rigidbody to this objects rigidbody so it follows
            _joint.connectedBody = _rb;
        }
    }

    //do the release magic
    void GripRelease(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //release the object if the joints rigidbody is the same as this objects rigidbody
        //so that if you grab and object with one had, then grab it with the other hand it wont drop when you let go with the first hand
        if (_joint != null && _joint.connectedBody == _rb && !_joint.gameObject.CompareTag("lockedInHand"))
        {
            GameObject go = _joint.gameObject;

            FireArm fa = go.GetComponent<FireArm>();
            if (fa != null)
            {
                //if there is a firearn script on the object
                //set the fireinput to something random, doesn't really matter and set the canfire to false
                fa.CanFire(false, SteamVR_Input_Sources.Head);
            }

            //set the collision detection mode back to discrete to save preformance
            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            //destroy the joint
            Object.DestroyImmediate(_joint);
            _joint = null;

            //remove it's parent
            go.transform.parent = null;


            /*  
            \/\/\/some wizardry copied from one of the examples in the steamVR plugin \/\/\/

            We should probably apply the offset between trackedObj.transform.position
            and device.transform.pos to insert into the physics sim at the correct
            location, however, we would then want to predict ahead the visual representation
            by the same amount we are predicting our render poses.
            */

            Transform origin = _trackedObj.origin ? _trackedObj.origin : _trackedObj.transform.parent;
            if (origin != null)
            {
                rigidbody.velocity = origin.TransformVector(_trackedObj.GetVelocity());
                rigidbody.angularVelocity = origin.TransformVector(_trackedObj.GetAngularVelocity());
            }
            else
            {
                rigidbody.velocity = _trackedObj.GetVelocity();
                rigidbody.angularVelocity = _trackedObj.GetAngularVelocity();
            }

            rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
        }
    }
}
