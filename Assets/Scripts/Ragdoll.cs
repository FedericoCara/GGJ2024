using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<MonoBehaviour> behaviours;
    [SerializeField] private Collider[] colliders;
    
    void Start()
    {
        SetEnabled(false);
    }

    void SetEnabled(bool enabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
        }
        foreach (var monoBehaviour in behaviours)
        {
            monoBehaviour.enabled = !enabled;
        }

        animator.enabled = !enabled;
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