using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour, DamageSystem.Callback
{
    Rigidbody rb;
    public void on_death()
    {

    }

    public void on_hit(Weapon with, DamageSystem.Type how)
    {
        Vector3 direction = transform.position - with.Holder.transform.position;
        direction.y = 0.0f;
        direction.Normalize();
        rb.AddForce(direction, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<DamageSystem>().register(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
