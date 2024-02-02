using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider; //refrence to our health sldier
    public float health; //our health value
    private Animator anim; // refrence to our animator 
    public string otherplayerName; //other player name string
    private void Start()
    {
        //set health and get slider and set the slider value to the health
        health = 100;
        healthSlider = GetComponentInChildren<Slider>();
        healthSlider.value = health;
        anim = GetComponent<Animator>(); //get the animator component
       
    }
    public void GainHealth()
    {
        if (health < 100)
        {
            //if our health is less than 100 add a randomvalue to our health
            float randomValue = Random.Range(10, 20);
            health += randomValue;
            healthSlider.value = health;
            health = Mathf.Min(health, 100); //make sure our health doesn't exceed 100
           
            Debug.Log("Added Health " + randomValue);
            Debug.Log(health);
        }
      
    }
    public void LoseHealth()
    {
        //lose a  randomvalue from our health
        float randomValue = Random.Range(20,30);
        health -= randomValue;
        healthSlider.value = health;

    }
    private void Update()
    {
        if (health <= 0)
        {
            //if we have less than zero health play the animation,disable the slider and call the endscreen after 2 seconds
            anim.SetTrigger("Dead");
            healthSlider.enabled = false;
            Invoke("EndScreen", 2f);
            // Destroy(this.gameObject, 2f);
        }
    }
    private void EndScreen()
    {
        //call the end of game from pvp manager and show that the other player won
        PVPManager.instance.EndOfGame();
        PVPManager.instance.winnerScreen.GetComponentInChildren<TextMeshProUGUI>().text = otherplayerName + " Won";
    }
}
