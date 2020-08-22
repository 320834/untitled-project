using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public float health = 100;
    public float stamina = 100;


    public Slider healthSlider;

    public Slider staminaSlider;

    public Text essence;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        staminaSlider.value = stamina;


        essence.text = GameState.essence.ToString();
        
    }

    public void decreaseHealth(float damage)
    {
        health = health - damage;
    }

    public void decreaseStamina(float value)
    {
        if(stamina >= 0)
        {
            stamina = stamina - value;
        }
    }

    public void increaseStamina(float value)
    {
        if(stamina <= 100)
        {
            stamina = stamina + value;
        }
    
    }

    public bool validStamina()
    {
        return stamina >= 0 && stamina <= 105;
    }

    public bool isAlive()
    {
        return this.health > 0;
    }
}
