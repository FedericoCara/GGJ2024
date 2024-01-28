using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabController : MonoBehaviour
{
    public event Action<FinishingBlowReceiver> OnImpactOnEnemy;
    
    [SerializeField] private Player _player;
    [SerializeField] private Transform handTransform;
    private List<GraspableRagdoll> _graspableAtHand = new();
    private GraspableRagdoll _grabbedRagdoll;

    public void LetGo()
    {
        _grabbedRagdoll.LetGo(transform.forward, transform.position);
        _grabbedRagdoll.Owner = null;
        _grabbedRagdoll = null;
    }

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

    private void Grab()
    {
        var target = GetClosestGrabbableRagdoll();
        if (target == null)
        {
            return;
        }

        _grabbedRagdoll = target;
        _grabbedRagdoll.SetGrabbedBy(handTransform);
        _grabbedRagdoll.Owner = _player;
    }

    private void OnTriggerEnter(Collider other)
    {
        var graspableRagdoll = other.GetComponentInParent<GraspableRagdoll>();
        if (graspableRagdoll != null && _grabbedRagdoll != graspableRagdoll)
        {
            _graspableAtHand.Add(graspableRagdoll);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var graspableRagdoll = other.GetComponentInParent<GraspableRagdoll>();
        _graspableAtHand.Remove(graspableRagdoll);
    }

    private GraspableRagdoll GetClosestGrabbableRagdoll()
    {
        float minDistance = float.MaxValue;
        GraspableRagdoll closestGrabbableRagdoll = null;
        for (int i = 0; i < _graspableAtHand.Count; i++)
        {
            // if (_graspableAtHand[i] == null)
            // {
            //     _graspableAtHand.RemoveAt(i--);
            //     continue;
            // }
            
            if (_grabbedRagdoll != _graspableAtHand[i])
            {
                var enemy = _graspableAtHand[i].GetComponent<Enemy>();
                if (enemy == null || enemy.IsDead)
                {
                    float distance = Vector3.Distance(_graspableAtHand[i].transform.position, handTransform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestGrabbableRagdoll = _graspableAtHand[i];
                    }
                }
            }
        }

        return closestGrabbableRagdoll;
    }
}
