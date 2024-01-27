using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<MonoBehaviour> behaviours;
    [SerializeField] private Collider[] colliders;
    [SerializeField] private Rigidbody normalRigidBody;
    [SerializeField] private Transform normalTransform;
    [SerializeField] private Transform ragdollTransform;
    [SerializeField] private ThirdPersonOrbitCamBasic normalCam;
    
    void Start()
    {
        SetEnabled(false);
    }
 
    void SetEnabled(bool enabled)
    {
        normalRigidBody.isKinematic = enabled;
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
        }
        foreach (var monoBehaviour in behaviours)
        {
            monoBehaviour.enabled = !enabled;
        }

        animator.enabled = !enabled;

        SwitchCam(enabled);
    }

    private void SwitchCam(bool enabled)
    {
        normalCam.player = enabled ? ragdollTransform : normalTransform;
        normalCam.ResetTargetOffsets();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetEnabled(true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetEnabled(false);
        }
    }
}