using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private Entity _owner;
    public Entity Owner
    {
        get => _owner;
        set => _owner = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_owner == null)
        {
            return;
        }

        Entity target = other.GetComponent<Entity>();
        if (target != null)
        {
            _owner.HandleWeaponCollision(target);
        }
    }
}
