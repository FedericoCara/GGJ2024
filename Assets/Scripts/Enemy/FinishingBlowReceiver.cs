using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FinishingBlowReceiver : MonoBehaviour
{
    [SerializeField] private RagdollEnabler _ragdollEnabler;
    [SerializeField] private Rigidbody hips;
    [SerializeField] private float normalKillStrength;
    [SerializeField] private float backhandKillStrength;
    [SerializeField] private float horizontalKillStrength;
    [SerializeField] private EventReference deathAudio;
    
    private Vector3 _forceVector;
    private bool impactReceived;

    public void KilledByNormalHit(Transform playerTransform)
    {
        if(impactReceived)
            return;
        KilledByHit(playerTransform);
        Debug.DrawLine(transform.position, transform.position+_forceVector*normalKillStrength, Color.red, 5);
        Debug.Log(name+" killed by normal hit");
        ApplyNormalForce();
    }


    public void KilledByBackhandHit(Transform playerTransform)
    {
        if(impactReceived)
            return;
        KilledByHit(playerTransform);
        _forceVector = CalculateForceVector(playerTransform);
        Debug.DrawLine(transform.position, transform.position+_forceVector*backhandKillStrength, Color.red, 5);
        Debug.Log(name+" killed by backhand hit");
        ApplyBackhandForce();
    }

    public void KilledByHorizontalHit(Transform playerTransform)
    {
        if(impactReceived)
            return;
        KilledByHit(playerTransform);
        _forceVector = CalculateForceVector(playerTransform);
        Debug.DrawLine(transform.position, transform.position+_forceVector*horizontalKillStrength, Color.red, 5);
        Debug.Log(name+" killed by horizontal hit");
        ApplyHorizontalForce();
    }

    private void KilledByHit(Transform playerTransform)
    {
        _ragdollEnabler.SetEnabled(true);
        impactReceived = true;
        _forceVector = CalculateForceVector(playerTransform);
        RuntimeManager.PlayOneShot(deathAudio);
    }

    private Vector3 CalculateForceVector(Transform playerTransform)
    {
        var forceVector = (transform.position-playerTransform.position);
        forceVector = new Vector3(forceVector.x, 0, forceVector.z);
        forceVector.Normalize();
        return forceVector;
    }

    private void ApplyNormalForce()
    {
        hips.AddForce((_forceVector+Vector3.down*0.3f).normalized * normalKillStrength, ForceMode.Impulse);
    }

    private void ApplyBackhandForce()
    {
        hips.AddForce(_forceVector * backhandKillStrength, ForceMode.Impulse);
    }

    private void ApplyHorizontalForce()
    {
        hips.AddForce((_forceVector+Vector3.up*0.3f).normalized * horizontalKillStrength, ForceMode.Impulse);
    }
}
