using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cookmeat : MonoBehaviour
{
    public int cookTime;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("MyTag");
        }
    }
 }
