using FMODUnity;
using UnityEngine;


public class MoveAudio : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private EventReference walkAudioStep;
    [SerializeField] private EventReference runAudioStep;
    private const float walkSpeed = 0.7f;
    private const float minRunSpeed = 0.9f;

    public void PlayWalkStep()
    {
        if(GetCurrentSpeed()<=walkSpeed)
        {
            //Debug.Log("PlayWalkStep");
            RuntimeManager.PlayOneShot(walkAudioStep);
        }
    }

    public void PlayRunStep()
    {
        if (GetCurrentSpeed() >= minRunSpeed)
        {
            //Debug.Log("PlayRunStep");
            RuntimeManager.PlayOneShot(runAudioStep);
        }
    }

    private float GetCurrentSpeed() => animator.GetFloat(GenericBehaviour.SpeedParameterHash);
}