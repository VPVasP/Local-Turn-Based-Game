using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public float health;
    private Animator anim;
    public string otherplayerName;
    private void Start()
    {
        health = 100;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.value = health;
        anim = GetComponent<Animator>();
    
    }
    public void GainHealth()
    {
        if (health < 100)
        {
            float randomValue = Random.Range(10, 20);
            health += randomValue;
            healthSlider.value = health;
            health = Mathf.Min(health, 100);
            Debug.Log("Added Health " + randomValue);
            Debug.Log(health);
        }
    }
    public void LoseHealth()
    {
        float randomValue = Random.Range(20,30);
        health -= randomValue;
        healthSlider.value = health;

    }
    private void Update()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
            healthSlider.enabled = false;
            Invoke("EndScreen", 2f);
            // Destroy(this.gameObject, 2f);
        }
    }
    private void EndScreen()
    {
        PVPManager.instance.EndOfGame();
        PVPManager.instance.winnerScreen.GetComponentInChildren<TextMeshProUGUI>().text = otherplayerName + " Won";
    }
}
