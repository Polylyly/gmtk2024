using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Camera : MonoBehaviour
{
    public float sensitivity;

    public Transform orientation, player, playerCapsule;
    public Rigidbody playerRigidbody;
    public CinemachineFreeLook cineCam;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject orientationObject = GameObject.Find("Orient");
        orientation = orientationObject.GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //sensitivity = PlayerPrefs.GetFloat("Mouse Sensitivity");

        cineCam.m_XAxis.m_MaxSpeed = sensitivity * 2;
        cineCam.m_YAxis.m_MaxSpeed = sensitivity / 33;

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
    }
}
