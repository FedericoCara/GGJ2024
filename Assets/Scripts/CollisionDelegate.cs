using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDelegate : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private void OnCollisionEnter(Collision collision)
    {
        if(target!=null)
            target.SendMessage(nameof(OnCollisionEnter),
                collision,
                SendMessageOptions.DontRequireReceiver);
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if(target!=null)
            target.SendMessage(nameof(OnCollisionExit),
                collision,
                SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target!=null)
            target.SendMessage(nameof(OnTriggerEnter),
                other,
                SendMessageOptions.DontRequireReceiver);
    }

    private void OnTriggerExit(Collider other)
    {
        if(target!=null)
            target.SendMessage(nameof(OnTriggerExit),
                other,
                SendMessageOptions.DontRequireReceiver);
    }
    
}
