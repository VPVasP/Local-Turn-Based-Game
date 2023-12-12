using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public float health;
    private void Start()
    {
        health = 100;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.value = health;
    }
    public void LoseHealth()
    {
        float randomValue = Random.Range(20,30);
        health -= randomValue;
        healthSlider.value = health;

    }
}
