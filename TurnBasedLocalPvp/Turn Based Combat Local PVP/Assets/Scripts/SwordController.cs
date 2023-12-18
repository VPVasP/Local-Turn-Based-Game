using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public string tag; //our tag of the gameobject we want to hit
    private Rigidbody rb; //refrence to the rigibody

    private void Start()
    {
        //get the rigibody component and freeze all constrains position and rotation
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(tag)) //if it collides with the tag we set up
        {
            collision.collider.GetComponent<PlayerHealth>().LoseHealth(); //we call the lose health to the object we hit
            Debug.Log("Dealt Damage " + collision.collider.name);
        }
      
    }
}
