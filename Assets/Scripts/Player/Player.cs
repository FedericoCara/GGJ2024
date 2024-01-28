using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    private AttackBehavior _attackBehavior;

    protected override float AttackDamage => _attackBehavior.AttackDamage;

    protected override void Awake()
    {
        base.Awake();
        _attackBehavior = GetComponent<AttackBehavior>();
    }

    protected override void OnAttackTargetDead(Entity target)
    {
        base.OnAttackTargetDead(target);
        var finishingBlowReceiver = target.GetComponent<FinishingBlowReceiver>();
        if (finishingBlowReceiver != null)
        {
            _attackBehavior.ApplyFinishingBlow(finishingBlowReceiver);
        }
    }
}
