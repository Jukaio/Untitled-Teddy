using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public GameObject healthbar;
    public int max_health = 100;
    public int regenerated_health = 10; // HP/sec
    public int combat_cooldown = 0; // health regenerates only when this is 0
    public int regeneration_cooldown = 5; // only start regenerating hp after x secs after combat

    public int curr_health;
    float regeneration_second = 0.0f;
    float combat_second = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        curr_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        // just to test
        curr_health = Mathf.Clamp(curr_health, 0, max_health);

        float health_percent = curr_health * 100 / max_health;
        healthbar.GetComponent<UnityEngine.UI.Slider>().value = health_percent / 100;
        if (health_percent > 50)
            GetComponent<UnityEngine.UI.Image>().color = new Color((100 - health_percent) / 50, 1, 0);
        else
            GetComponent<UnityEngine.UI.Image>().color = new Color(1, health_percent / 50, 0);
        // just to test end

        if (combat_cooldown == 0)
        {
            if(curr_health < max_health)
                regeneration_second += Time.deltaTime;
        }
        else
        {
            combat_second += Time.deltaTime;
            if (combat_second >= 1.0f)
            {
                combat_second = 0.0f;
                combat_cooldown--;
            }
        }

        if (regeneration_second >= 1.0f)
        {
            regeneration_second = 0.0f;
            curr_health += regenerated_health;
        }
    }
    void change_health(int value)
    {
        if(value < 0)
        {
            combat_cooldown = regeneration_cooldown;
        }
        curr_health += value;
        curr_health = Mathf.Clamp(curr_health, 0, max_health);

        float health_percent = curr_health * 100 / max_health;
        healthbar.GetComponent<UnityEngine.UI.Slider>().value = health_percent / 100;
        if (health_percent > 50)
            GetComponent<UnityEngine.UI.Image>().color = new Color((100 - health_percent) / 50, 1, 0);
        else
            GetComponent<UnityEngine.UI.Image>().color = new Color(1, health_percent / 50, 0);

        if (curr_health == 0)
        {
            // die?
        }
    }
}
