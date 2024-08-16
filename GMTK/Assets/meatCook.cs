using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatCook : MonoBehaviour
{
    public Material[] material;
    private Renderer rend;

    public int cookTime;

    public bool waited = true;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.sharedMaterial = material[0];
    }

    void OnCollisionStay(Collision collision)
    {
        if ( cookTime == 10)
        {
            rend.sharedMaterial = material[1];
        }

        if (cookTime == 15)
        {
            rend.sharedMaterial = material[2];
        }

        if (cookTime == 20)
        {
            rend.sharedMaterial = material[3];
        }


        if (collision.collider.tag == "Player")
        {
            Debug.Log("Taged");
            if (waited == true)
            {
                StartCoroutine(waiter());

                Debug.Log("entering waiter");
            } 
        }
    }

    IEnumerator waiter()
    {
        waited = false;
        yield return new WaitForSeconds(1);
        cookTime += 1;
        Debug.Log(cookTime);
        waited = true;
    }
}
