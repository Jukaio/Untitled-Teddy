using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(HealthSystem))]
public class DamageSystem : MonoBehaviour
{
    HealthSystem health;

    public enum Type
    {
        Collision,
        Trigger
    }

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
    }

    public interface Callback
    {
        public abstract void on_hit(Weapon with, Type how);
        public abstract void on_death();
    }
    List<Callback> callbacks = new List<Callback>();
    public void register(Callback that)
    {
        callbacks.Add(that);
    }
    public void unregister(Callback that)
    {
        callbacks.Remove(that);
    }


    public void on_death()
    {
        foreach(var callback in callbacks)
        {
            callback.on_death();
        }
    }

    public void on_hit(Weapon with, Type how)
    {
        foreach (var callback in callbacks)
        {
            callback.on_hit(with, how);
        }
        damage(with.get_damage());
    }

    public void damage(int that)
    {
        int var = Mathf.Clamp(that, 0, int.MaxValue);
        health.change_health(-that);
        if (health.IsDead)
            on_death();
    }

    private void OnTriggerEnter(Collider other)
    {
        Sword weapon = null;
        if(other.gameObject.TryGetComponent(out weapon))
        {
            on_hit(weapon, Type.Trigger);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Sword weapon = null;
        if (collision.gameObject.TryGetComponent(out weapon))
        {
            on_hit(weapon, Type.Collision);
        }
    }
}
