using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity
{
    [SerializeField]
    private float _speedAnimationMultiplier = 10f;

    [SerializeField]
    private float _range = 1f;

    [SerializeField]
    private float _attack = 20;

    [SerializeField]
    private float _attackCoolDown = 2f;

    private NavMeshAgent _agent;

    private GameObject _player;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    Vector3 oldPosition;

    private float _attackCoolDownTimeRemaining;

    private bool _isAttacking;

    protected override float AttackDamage => _attack;

    protected override bool IsAttacking => _isAttacking;

    public override void HandleWeaponCollision(Entity target)
    {
        if (target is Player)
        {
            base.HandleWeaponCollision(target);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        Animator.SetBool("Interrupt to Take Hit", true);
    }  

    protected void Update()
    {
        if (IsDead)
        {
            return;
        }

        if (IsGrounded())
        {
            if (_player == null)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }

            _attackCoolDownTimeRemaining -= Time.deltaTime;
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
                            Animator.SetFloat(GenericBehaviour.SpeedParameterHash, _agent.velocity.magnitude);
                            _agent.nextPosition = Animator.rootPosition;
                        }
                        else
                        {
                            Animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
                            _agent.SetDestination(transform.position);

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
                        OnPlayerInRange();
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
		    Animator.SetBool(BasicBehaviour.GroundedParameterHash, false);
		    Animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
            _agent.destination = transform.position;
        }
        oldPosition=transform.position;
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
    public void OnAttackFinished()
    {
        _isAttacking = false;
        //Debug.Log("On Attack Finished for "+name);
    }

    protected override void Die()
    {
        base.Die();
        _agent.enabled = false;
    }

    private void OnPlayerInRange()
    {
        Animator.SetFloat(GenericBehaviour.SpeedParameterHash, 0, speedDampTime, Time.deltaTime);
        _agent.SetDestination(transform.position);

        if (_attackCoolDownTimeRemaining < 0)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Animator.SetTrigger(AttackBehavior.AttackNormal);
        _isAttacking = true;
        _attackCoolDownTimeRemaining = _attackCoolDown;
    }
}
