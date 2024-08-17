using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jamMaker : MonoBehaviour
{
    public LayerMask strawbarry;
    public LayerMask sugarCube;
    public GameObject jamLayer;
    public int stawbarryInside = 0;
    public int sugarInside = 0;

    private HashSet<GameObject> objectsInside = new HashSet<GameObject>();


    private void OnTriggerEnter(Collider other)
    {

        if (((1 << other.gameObject.layer) & strawbarry) != 0)
        {

            stawbarryInside++;
            Debug.Log("stawbary inside " + stawbarryInside);
        }

        if (((1 << other.gameObject.layer) & sugarCube) != 0)
        {

            sugarInside++;
            Debug.Log("sugar cube inside " + sugarInside);
        }

        if (sugarInside == 3 && stawbarryInside == 2)
        {
            jamLayer.gameObject.SetActive(true);

            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Check if the object is on the specified layer
                if (((1 << obj.layer) & strawbarry) != 0)
                {
                    // Destroy the object
                    Destroy(obj);
                    Debug.Log("Destroyed object: " + obj.name);
                }
            }
            foreach (GameObject obj in allObjects)
            {
                // Check if the object is on the specified layer
                if (((1 << obj.layer) & sugarCube) != 0)
                {
                    // Destroy the object
                    Destroy(obj);
                    Debug.Log("Destroyed object: " + obj.name);
                }
            }
        }
    


    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object is on the specified layer
        if (((1 << other.gameObject.layer) & strawbarry) != 0)
        {
            // Decrement the count when an object exits
            stawbarryInside--;
            Debug.Log("strawbarry inside " + stawbarryInside);
        }

        if (((1 << other.gameObject.layer) & sugarCube) != 0)
        {
            // Decrement the count when an object exits
            sugarInside--;
            Debug.Log("sugar cube inside " + sugarInside);
        }
    }

    public int GetObjectsInsideCount()
    {
        return stawbarryInside;
        return sugarInside;
    }

   
}
