using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHandle : WeaponHandle
{
    [SerializeField] Sword sword;
    [SerializeField] private Animator anim;

    [SerializeField] GameObject main;
    [SerializeField] GameObject axe;
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject cool_axe;
    [SerializeField] GameObject hammer;

    [SerializeField] GameObject UI;
    bool has_weapon = true;
    public bool HasWeapon { get{ return has_weapon; }}

    [FMODUnity.EventRef]
    public string SwingEvent = "";

    public void lose_weapon()
    {
        has_weapon = false;
        UI.SetActive(!false);
    }
    private void deactivate_all()
    {
        main.SetActive(false);
        axe.SetActive(false);
        pistol.SetActive(false);
        cool_axe.SetActive(false);
        hammer.SetActive(false);
    }
    public void get_weapon(string name)
    {
        deactivate_all();
        if(name.Contains("sword"))
        {
            main.SetActive(true);
        }
        if (name.Contains("axe"))
        {
            axe.SetActive(true);
        }
        if (name.Contains("pistol"))
        {
            pistol.SetActive(true);
        }
        if (name.Contains("weapon"))
        {
            cool_axe.SetActive(true);
        }
        if (name.Contains("hammer"))
        {
            cool_axe.SetActive(true);
        }
        UI.SetActive(!true);
        has_weapon = true;
    }

    public override void on_hold()
    {
        
    }
    public override void on_press()
    {
        if (has_weapon)
        {
            anim.SetTrigger("Attack!");
            FMODUnity.RuntimeManager.PlayOneShot(SwingEvent, transform.position);
        }
    }
    public override void on_release()
    {
        
    }
}
