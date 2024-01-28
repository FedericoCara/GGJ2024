using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspableRagdoll : Weapon
{
    [SerializeField] private int _hitsBeforeDestruction = 5;
    [SerializeField] private int _remainingHitsBeforeDestruction;
    [SerializeField] private Transform hips;
    [SerializeField] private Transform grabbingPoint;
    [SerializeField] private GameObject _destructionEffectPrefab;

    private BoxCollider _collider;
    private List<Rigidbody> _rigidbodies = new();
    private List<Collider> _colliders = new();
    private int _defaultLayer;
    private int _grabbedLayer;

    private void Awake()
    {
        _defaultLayer = gameObject.layer;
        _grabbedLayer = LayerMask.NameToLayer("Grabbed Ragdoll");
        _remainingHitsBeforeDestruction = _hitsBeforeDestruction;
    }

    public void SetGrabbedBy(Transform handTransform)
    {
        EnablePhysics(false);        
        transform.SetParent(handTransform);
        transform.position = handTransform.position - grabbingPoint.position + transform.position;
        SetLayer(_grabbedLayer);
    }

    public void LetGo(Vector3 grabberForward, Vector3 transformPosition)
    {
        EnablePhysics(true);
        transform.SetParent(null);
        var position = transform.position;
        hips.localPosition = Vector3.zero;
        transform.position = new Vector3(position.x,
             Mathf.Max(transformPosition.y+1f,position.y),
                                        position.z) + grabberForward;
        SetLayer(_defaultLayer);
    }

    protected override bool OnCollisionWithEntity(Entity target)
    {
        if (base.OnCollisionWithEntity(target))
        {
            if (--_remainingHitsBeforeDestruction <= 0)
            {
                var player = Owner as Player;
                player.RemoveEquippedRagdoll();
                Instantiate(_destructionEffectPrefab, hips.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            return true;
        }

        return false;
    }

    private void EnablePhysics(bool enable = true)
    {
        foreach (var rb in GetRigidbodies())
        {
            rb.isKinematic = !enable;
        }

        foreach (var col in GetColliders())
        {
            col.isTrigger = !enable;
        }
    }

    private List<Rigidbody> GetRigidbodies()
    {
        _rigidbodies.Clear();
        _rigidbodies.AddRange(GetComponentsInChildren<Rigidbody>());
        return _rigidbodies;
    }

    private List<Collider> GetColliders()
    {
        _colliders.Clear();
        _colliders.AddRange(GetComponentsInChildren<Collider>());
        return _colliders;
    }

    private void SetLayer(int layer)
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layer;
        }
    }
}
