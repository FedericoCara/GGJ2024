using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspableRagdoll : MonoBehaviour
{
    [SerializeField] private Transform hips;
    [SerializeField] private Transform grabbingPoint;

    public event Action<Collider> OnCollidingWithSomething;

    private BoxCollider _collider;
    private List<Rigidbody> _rigidbodies = new();
    private List<Collider> _colliders = new();
    private int _defaultLayer;
    private int _grabbedLayer;

    private void Awake()
    {
        _defaultLayer = gameObject.layer;
        _grabbedLayer = LayerMask.NameToLayer("Grabbed Ragdoll");
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

    public void OnTriggerEnter(Collider other)
    {
        OnCollidingWithSomething?.Invoke(other);
    }
}
