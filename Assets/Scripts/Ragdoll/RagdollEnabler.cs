using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private bool allowInput;
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
 
    public void SetEnabled(bool enabled)
    {
        normalRigidBody.isKinematic = enabled;
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
            collider.GetComponent<Rigidbody>().isKinematic = !enabled;
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
        if(normalCam==null)
            return;
        normalCam.player = enabled ? ragdollTransform : normalTransform;
        normalCam.ResetTargetOffsets();
    }

    void Update()
    {
        if(!allowInput)
            return;
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetEnabled(true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetEnabled(false);
        }
    }
}