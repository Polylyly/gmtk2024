using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jamMaker : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "straw");
        {
           
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "straw")
        {
            
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
}
