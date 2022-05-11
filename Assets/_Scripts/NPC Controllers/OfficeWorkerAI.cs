using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OfficeWorkerAI : MonoBehaviour
{
    //config variables
    [SerializeField] private Transform[] _escapePoint;
    [SerializeField] private float _escapeSpeed = 10;
    [SerializeField] private Vector3 _maxPos, _minPos;
    [SerializeField] private float _maxWaitTime, _minWaitTime;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Animator _anim;
    [SerializeField] private float animationSpeedMult = 0.3f;

    private bool _escaping;
    private float _waitTime;
    private float _prevTime;

    private NavMeshAgent _agent;
    private Rigidbody _rb;


// Start is called before the first frame update
void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _waitTime = Random.Range(_minWaitTime, _maxWaitTime);
        _prevTime = Time.time;

        NewRandomPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //set a new random position after a set random time
        if(Time.time > _prevTime + _waitTime && _agent.enabled && !_escaping)
        {
            _waitTime = Random.Range(_minWaitTime, _maxWaitTime);
            _prevTime = Time.time;
            NewRandomPosition();
        }
        
        //dissable agent and set rigidbody to not be kinematic if player picks up worker
        if(GetComponent<ConfigurableJoint>() != null)
        {
            _agent.enabled = false;
            _rb.isKinematic = false;
            _anim.SetFloat("Speed", 0);
        }
        else
        {
            _anim.SetFloat("Speed", _agent.velocity.magnitude * animationSpeedMult);
        }


        //dissable agent, gravity and set kinematic to false when the worker leaves the window. also set forward velocity to 10 and set a random rotation
        if (_escaping && !Physics.Raycast(transform.position + (-transform.forward * 1f), -transform.up, 2, _groundLayer) && _agent.enabled)
        {
            _agent.enabled = false;
            _rb.isKinematic = false;
            _rb.useGravity = false;

            _rb.velocity = transform.forward * 10;
            transform.rotation = new Quaternion(Random.Range(-45, 45), Random.Range(135 , 225), Random.Range(-45, 45), transform.rotation.w);
        }
    }

    //get a new random position within the vector positions _minPos and _maxPos
    private void NewRandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(_minPos.x, _maxPos.x), Random.Range(_minPos.y, _maxPos.y), Random.Range(_minPos.z, _maxPos.z));
        _agent.destination = position;
        //Debug.DrawRay(position, transform.up * 10, Color.green, _waitTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //re-enable agent and set rigidbody to kinematic when worker hits ground afet being picked up and thrown/let go
        if(collision.gameObject.layer == 8 && GetComponent<ConfigurableJoint>() == null && !_escaping)
        {
            _agent.enabled = true;
            _rb.isKinematic = true;
        }
    }

    public void Escape()
    {
        //start runing towards the escape point
        if (!_escaping)
        {
            _escaping = true;
            _agent.speed = _escapeSpeed;
            _agent.destination = _escapePoint[Random.Range(0, _escapePoint.Length)].position;
        }
    }
}
