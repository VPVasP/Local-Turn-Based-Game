using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public string tag;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(tag))
        {
            collision.collider.GetComponent<PlayerHealth>().LoseHealth();
            Debug.Log("Dealt Damage " + collision.collider.name);
        }
      
    }
}
