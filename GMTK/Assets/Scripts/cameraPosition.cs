using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraPosition : MonoBehaviour
{
    public Transform Position;

    private void Start()
    {
        GameObject positionObject = GameObject.Find("CameraPos");
        Position = positionObject.GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Position.position;
    }
}
