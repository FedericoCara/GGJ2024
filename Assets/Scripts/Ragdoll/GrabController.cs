using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public event Action<FinishingBlowReceiver> OnImpactOnEnemy;
    
    [SerializeField] private Player _player;
    [SerializeField] private Transform handTransform;
    private GraspableRagdoll _graspableAtHand;
    private GraspableRagdoll _grabbedRagdoll;

    private void Update()
    {
        if (Input.GetButtonDown("Grab"))
        {
            if (_grabbedRagdoll!=null)
            {
                LetGo();
            }
            else if(_graspableAtHand!=null)
            {
                Grab();
            }
        }
    }

    private void LetGo()
    {
        _grabbedRagdoll.LetGo(transform.forward, transform.position);
        _grabbedRagdoll.Owner = null;
        _grabbedRagdoll = null;
    }

    private void Grab()
    {
        _grabbedRagdoll = _graspableAtHand;
        _grabbedRagdoll.SetGrabbedBy(handTransform);
        _grabbedRagdoll.Owner = _player;
    }

    private void HandleWeaponCollision(Collider collider)
    {
        var impactReceiver = collider.GetComponentInParent<FinishingBlowReceiver>();
        if(impactReceiver!=null)
            OnImpactOnEnemy?.Invoke(impactReceiver);
    }

    private void OnTriggerEnter(Collider other)
    {
        var graspableRagdoll = other.GetComponentInParent<GraspableRagdoll>();
        if (graspableRagdoll != null && _grabbedRagdoll!=graspableRagdoll)
        {
            _graspableAtHand = graspableRagdoll;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var graspableRagdoll = other.GetComponentInParent<GraspableRagdoll>();
        if (graspableRagdoll == _graspableAtHand)
        {
            _graspableAtHand = null;
        }
    }
}
