using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meatCook : MonoBehaviour
{
    public Material[] material;
    public Renderer rend;
    public GameObject vfx;


    public int cookTime;
    private bool isCookig;
    private bool cookWait;

    public bool waited = true;

    private void Start()
    {
     
        rend.sharedMaterial = material[0];
    }

    void OnTriggerStay(Collider target)
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

        if (target.tag != "Stove" && isCookig == true && cookWait == true)
        {
            
                StartCoroutine(burning());
           
        }

            if (target.tag == "Stove")
        {

            vfx.gameObject.SetActive(true);
            isCookig = true;
            cookWait = false;

            Debug.Log("Taged");
            if (waited == true)
            {
                StartCoroutine(waiter());

                Debug.Log("entering waiter");
            }
            else
                StartCoroutine(anotherWaiter());
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

    IEnumerator burning()
    {
        yield return new WaitForSeconds(3);
        vfx.gameObject.SetActive(false);

        isCookig = false; 
    }

    IEnumerator anotherWaiter()
    {
        yield return new WaitForSeconds(1);
        cookWait = true;
    }
}
