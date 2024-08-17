using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamJump : MonoBehaviour
{
    
    public int jump_amount;
    public bool Jam_ready = false;

    public LayerMask container;


    public GameObject button;

    private void OnCollisionEnter(Collision collision)
    {
        if (jump_amount <= 5)
            gameObject.transform.localScale += new Vector3(0, 1, 0);
        jump_amount += 1;

        if (jump_amount > 5)
            Jam_ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & container) != 0 && jump_amount >=5)
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (((1 << other.gameObject.layer) & container) != 0 )
        {
            button.SetActive(false);
        }
    }
}
