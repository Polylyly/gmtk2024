using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class butan : MonoBehaviour
{
    public GameObject jam;
    private void OnCollisionEnter(Collision collision)
    {
        jam.gameObject.SetActive(true);  
    }
}
