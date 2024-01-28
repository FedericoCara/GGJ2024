using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
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
