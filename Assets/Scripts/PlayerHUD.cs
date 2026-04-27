using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    
    public PlayerController player;
    public Slider healthSlider;
    public TMP_Text healthText;

    void Start()
    {
        healthSlider.maxValue = player.maxHealth;
        healthSlider.value = player.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        healthSlider.value = Mathf.Clamp(player.currentHealth, 0, player.maxHealth);
        healthText.text = player.currentHealth + " / " + player.maxHealth;

    }
}
