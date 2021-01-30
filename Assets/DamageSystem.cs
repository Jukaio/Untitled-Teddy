using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DamageSystem : MonoBehaviour
{
    [SerializeField] private int max_health;
    private int current_health;

    public enum Type
    {
        Collision,
        Trigger
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
        current_health -= that;
        if (current_health <= 0)
            on_death();
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = null;
        if(other.gameObject.TryGetComponent(out weapon))
        {
            on_hit(weapon, Type.Trigger);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Weapon weapon = null;
        if (collision.gameObject.TryGetComponent(out weapon))
        {
            on_hit(weapon, Type.Collision);
        }
    }
}
