using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    public static readonly int AttackNormal = Animator.StringToHash("AttackNormal");
    public static readonly int AttackBackhand = Animator.StringToHash("AttackBackhand");
    public static readonly int AttackHorizontal = Animator.StringToHash("AttackHorizontal");
    [SerializeField] private GrabController grabController;

    [SerializeField]
    private float _normalAttackDamage = 40;

    [SerializeField] private EventReference normalAttackAudio;
    
    [SerializeField]
    private float _horizontalAttackDamage = 60;
    
    [SerializeField] private EventReference horizontalAttackAudio;

    [SerializeField]
    private float _backhandAttackDamage = 100;
    
    [SerializeField] private EventReference backhandAttackAudio;
    
    private Animator _animator;
    private bool _waitingToResetTriggers;
    private bool _doingNormalAttack;
    private bool _doingHorizontalAttack;
    private bool _doingBackhandAttack;
    
    public float AttackDamage
    {
        get
        {
            if (_doingNormalAttack)
            {
                return _normalAttackDamage;
            }
            else if (_doingHorizontalAttack)
            {
                return _horizontalAttackDamage;
            }
            else if (_doingBackhandAttack)
            {
                return _backhandAttackDamage;
            }
            else
            {
                return 0;
            }
        }
    }
    
    public bool IsAttacking => _doingNormalAttack || _doingHorizontalAttack || _doingHorizontalAttack;

    public void ApplyFinishingBlow(FinishingBlowReceiver receiver)
    {
        if(_doingNormalAttack)
            receiver.KilledByNormalHit(transform);
        else if(_doingBackhandAttack)
            receiver.KilledByBackhandHit(transform);
        else if(_doingHorizontalAttack)
            receiver.KilledByHorizontalHit(transform);
    }

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(_waitingToResetTriggers)
            return;
        
        if(WasAttacking() && !IsAttackState())
            OnAttackFinished();


        if (Input.GetButtonDown("Fire1"))
            PerformNormalAttack();
        else if (Input.GetButtonDown("Fire2"))
            PerformBackhandAttack();
        else if (Input.GetButtonDown("Fire3")) PerformHorizontalAttack();
    }

    private bool WasAttacking() => 
        _doingNormalAttack || _doingBackhandAttack || _doingHorizontalAttack;

    private bool IsAttackState()
    {
        if (_animator.IsInTransition(0))
        {
            var nextState = _animator.GetNextAnimatorStateInfo(0);
            if (nextState.IsName("Attack Normal") ||
                nextState.IsName("Attack Backhand") ||
                nextState.IsName("Attack Horizontal"))
                return true;
        }

        var currentState = _animator.GetCurrentAnimatorStateInfo(0);
        return currentState.IsName("Attack Normal") ||
               currentState.IsName("Attack Backhand") ||
               currentState.IsName("Attack Horizontal");
    }


    public void OnAttackFinished()
    {
        _doingNormalAttack = false;
        _doingBackhandAttack = false;
        _doingHorizontalAttack = false;
    }

    private void PerformNormalAttack()
    {
        _animator.SetTrigger(AttackNormal);
        ResetTriggersAfterSomeMilliseconds();
        _doingNormalAttack = true;
        RuntimeManager.PlayOneShot(normalAttackAudio);
    }

    private void PerformHorizontalAttack()
    {
        _animator.SetTrigger(AttackHorizontal);
        ResetTriggersAfterSomeMilliseconds();
        _doingHorizontalAttack = true;
        RuntimeManager.PlayOneShot(horizontalAttackAudio);
    }

    private void PerformBackhandAttack()
    {
        _animator.SetTrigger(AttackBackhand);
        ResetTriggersAfterSomeMilliseconds();
        _doingBackhandAttack = true;
        RuntimeManager.PlayOneShot(backhandAttackAudio);
    }

    private void ResetTriggersAfterSomeMilliseconds()
    {
        StartCoroutine(ResetTriggersAfterSomeMillisecondsCoroutine());
    }

    private IEnumerator ResetTriggersAfterSomeMillisecondsCoroutine()
    {
        _waitingToResetTriggers = true;
        yield return new WaitForSeconds(0.02f);
        
        _animator.ResetTrigger(AttackNormal);
        _animator.ResetTrigger(AttackBackhand);
        _animator.ResetTrigger(AttackHorizontal);
        
        _waitingToResetTriggers = false;
    }
}
