using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHandle : WeaponHandle
{
    [SerializeField] Sword sword;
    [SerializeField] private Animator anim;

    public override void on_hold()
    {
        
    }
    public override void on_press()
    {
        anim.SetTrigger("Attack!");
    }
    public override void on_release()
    {
        
    }
}
