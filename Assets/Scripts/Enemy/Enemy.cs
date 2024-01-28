using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.

    [SerializeField]
    private float _speedAnimationMultiplier = 10f;

    [SerializeField]
    private float _range = 1f;
	
    private Vector3 colExtents;

    private Collider _collider;

    private Rigidbody _rigidBody;

    private Animator _animator;

    private NavMeshAgent _agent;

    private GameObject _player;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Vector3 oldPosition;
    protected void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
		colExtents = _collider.bounds.extents;
        _agent.updatePosition = false;
        _agent.updateRotation = false;
    }  

    protected void Update()
    {
        if (IsGrounded())
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
float speed = 0;
            // if (_player == null)
            // {
            //     _agent.destination = transform.position;
            // }
            // else
            // {
                // if (Vector3.Distance(_player.transform.position, transform.position) > _range)
                // {
                    // Vector3 worldDeltaPosition = _agent.nextPosition - transform.position;

                    // // Map 'worldDeltaPosition' to local space
                    // float dx = Vector3.Dot (transform.right, worldDeltaPosition);
                    // float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
                    // Vector2 deltaPosition = new Vector2 (dx, dy);

                    // // Low-pass filter the deltaMove
                    // float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
                    // smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

                    // // Update velocity if time advances
                    // if (Time.deltaTime > 1e-5f)
                    //     velocity = smoothDeltaPosition / Time.deltaTime;

                    // bool shouldMove = velocity.magnitude > 0.5f && _agent.remainingDistance > _range;

                    if (Vector3.Distance(_player.transform.position, transform.position) > _range)
                    {
                        _agent.SetDestination(_player.transform.position);
                        Vector3 worldDeltaPosition = _agent.nextPosition - _player.transform.position;

                        // Map 'worldDeltaPosition' to local space
                        float dx = Vector3.Dot (transform.right, worldDeltaPosition);
                        float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
                        Vector2 deltaPosition = new Vector2 (dx, dy);

                        // Low-pass filter the deltaMove
                        float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
                        smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

                        // Update velocity if time advances
                        if (Time.deltaTime > 1e-5f)
                            velocity = smoothDeltaPosition / Time.deltaTime;

                        bool shouldMove = velocity.magnitude > 0.5f && _agent.remainingDistance > _range;

                        if (_agent.velocity.magnitude > 0.1)
                        {
                        _animator.SetFloat(GenericBehaviour.SpeedParameterHash, _agent.velocity.magnitude);
                    }
                    else
                    {
		    _animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
                    }
                //         if (shouldMove)
                //         {
                //     // Update animation parameters
		        //         //_animator.SetFloat(GenericBehaviour.SpeedParameterHash, velocity.magnitude * _speedAnimationMultiplier, speedDampTime, Time.deltaTime);
		        //         //_animator.SetFloat(GenericBehaviour.SpeedParameterHash, 1);     
                //         _animator.SetFloat(GenericBehaviour.SpeedParameterHash, _agent.velocity.magnitude);   
                //         }            
                //         else
                //         {
                // _animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
                //         _agent.SetDestination(transform.position);
                //         }     
                    }
                    else
                    {
		    _animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
                    _agent.SetDestination(transform.position);
//transform.position = oldPosition;
                    }
                // }
                // else
                // {
                //     _agent.destination = transform.position;
                // }

                transform.LookAt(_player.transform);
            // }

		    // _animator.SetBool(BasicBehaviour.GroundedParameterHash, true);
		    // _animator.SetFloat(GenericBehaviour.SpeedParameterHash, speed, speedDampTime, Time.deltaTime);
        }
        else
        {
		    _animator.SetBool(BasicBehaviour.GroundedParameterHash, false);
		    _animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
            _agent.destination = transform.position;
        }
    oldPosition=transform.position;
   _agent.nextPosition = _animator.rootPosition;
    }

// //         if (velocity.magnitude > 0.5f && _agent.remainingDistance > _range)
// //         {
// //         // Update position to agent position
// //         // transform.position = _agent.nextPosition;
// //     Vector3 position = _animator.rootPosition;
// //     position.y = _agent.nextPosition.y;
// //     transform.position = position;
// //     _agent.nextPosition = transform.position;
// //     oldPosition=transform.position;
// //         }
// //         else
// //         {
// transform.position = _agent.nextPosition;
// //         }
//     }

// void OnAnimatorMove () {
//     Vector3 position = _animator.rootPosition;
//     position.y = _agent.nextPosition.y;
//     transform.position = position;
//     _agent.nextPosition = transform.position;
// }

	// Function to tell whether or not the player is on ground.
	public bool IsGrounded()
	{
		Ray ray = new Ray(this.transform.position + Vector3.up * (2 * colExtents.x), Vector3.down);
		return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f);
	}
}
