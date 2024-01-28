using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    private GrabController _grabController;

    private MoveBehaviour _moveBehavior;

    private AttackBehavior _attackBehavior;

    private RagdollEnabler _ragdollEnabler;

    private Action<float> _onTakeDamage;

    protected override float AttackDamage => _attackBehavior.AttackDamage;

    protected override bool IsAttacking => _attackBehavior.IsAttacking;
    protected override bool HasAlreadyHitInThisAttack(Entity target) => target is Enemy enemyTarget &&
                                                                        _attackBehavior.EnemiesHitDuringBlow.Contains(enemyTarget);

    public void SetOnTakeDamageAction(Action<float> onTookDamage)
    {
        _onTakeDamage = onTookDamage;
    }

    public override bool TakeDamage(float damage)
    {
        var result = base.TakeDamage(damage);
        _onTakeDamage?.Invoke(_hp / _maxHP);
        return result;
    }

    public void RemoveEquippedRagdoll()
    {
        _grabController.LetGo();
    }

    protected override void Awake()
    {
        base.Awake();
        _moveBehavior = GetComponent<MoveBehaviour>();
        _attackBehavior = GetComponent<AttackBehavior>();
        _ragdollEnabler = GetComponent<RagdollEnabler>();
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

    protected override void Die()
    {
        base.Die();
        _moveBehavior.enabled = false;
        if(_grabController!=null)
            _grabController.LetGo();
        _ragdollEnabler.SetEnabled(true);
    }
}
