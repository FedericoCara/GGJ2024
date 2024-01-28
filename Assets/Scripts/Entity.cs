using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.

    private static readonly int TakeHitTriggerID = Animator.StringToHash("Take Hit");

    [SerializeField]
    private float _maxHP = 100;
	
    private Vector3 colExtents;

    private Animator _animator;

    private float _hp;

    private bool _isAttacking;

    protected abstract float AttackDamage { get; }

    protected bool IsAttacking => _isAttacking;

    protected Animator Animator => _animator;
    
    public void HandleWeaponCollision(Entity target)
    {
        if (target.TakeDamage(AttackDamage))
        {
            OnAttackTargetDead(target);
        }
    }

    public bool TakeDamage(float damage)
    {
        if (_hp > 0)
        {
            _hp = Mathf.Max(_hp - damage, 0);
            if (_hp > 0)
            {
                _animator.SetTrigger(TakeHitTriggerID);
                return false;
            }
            else
            {
                Die();
                return true;
            }
        }

        return false;
    }

    public virtual void OnAttackAnimationFinished()
    {
        _isAttacking = false;
    }

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
		colExtents = GetComponent<Collider>().bounds.extents;
        ///_ragdoll = GetComponent<Ragdoll>();
        _hp = _maxHP;
    }

	// Function to tell whether or not the player is on ground.
	protected bool IsGrounded()
	{
		Ray ray = new Ray(transform.position + Vector3.up * (2 * colExtents.x), Vector3.down);
		return Physics.SphereCast(ray, colExtents.x, colExtents.x + 0.2f);
	}

    protected void Attack(string animationTrigger)
    {
        _animator.SetTrigger(animationTrigger);
        _isAttacking = true;
    }

    protected virtual void OnAttackTargetDead(Entity target)
    {

    }

    protected virtual void Die()
    {
        //_ragdoll.SetEnabled(true);
    }
}
