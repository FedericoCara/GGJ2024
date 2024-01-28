using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    private Animator _animator;
    private static readonly int AttackNormal = Animator.StringToHash("AttackNormal");
    private static readonly int AttackBackhand = Animator.StringToHash("AttackBackhand");
    private static readonly int AttackHorizontal = Animator.StringToHash("AttackHorizontal");
    private bool _waitingToResetTriggers;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        if(_waitingToResetTriggers)
            return;
        
        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger(AttackNormal);
            ResetTriggersAfterSomeMilliseconds();
        }else if (Input.GetButtonDown("Fire2"))
        {
            _animator.SetTrigger(AttackBackhand);
            ResetTriggersAfterSomeMilliseconds();
        }else if (Input.GetButtonDown("Fire3"))
        {
            _animator.SetTrigger(AttackHorizontal);
            ResetTriggersAfterSomeMilliseconds();
        }
    }

    private void ResetTriggersAfterSomeMilliseconds()
    {
        StartCoroutine(ResetTriggersAfterSomeMillisecondsCoroutine());
    }

    private IEnumerator ResetTriggersAfterSomeMillisecondsCoroutine()
    {
        _waitingToResetTriggers = true;
        yield return new WaitForSeconds(0.015f);
        
        _animator.ResetTrigger(AttackNormal);
        _animator.ResetTrigger(AttackBackhand);
        _animator.ResetTrigger(AttackHorizontal);
        
        _waitingToResetTriggers = false;
    }

    private void TurnOffAllTriggers()
    {
        _animator.SetBool(AttackNormal, false);
        _animator.SetBool(AttackBackhand, false);
        _animator.SetBool(AttackHorizontal, false);
    }
}
