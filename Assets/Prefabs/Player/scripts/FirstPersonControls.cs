using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControls : MonoBehaviour {

    public Rigidbody rb;
    public float magnitude = 1F;
    public float jump = 10F;
    public Transform Camera;

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.W)) {
            rb.AddForce(Camera.forward * magnitude * rb.mass);
            Debug.Log(Camera.forward * magnitude * rb.mass);
        }

        if (Input.GetKey(KeyCode.A)) {
            rb.AddForce(Camera.forward.magnitude * rb.mass, 0, 0);
        }

        if (Input.GetKey(KeyCode.S)) {
            rb.AddForce(0, 0, magnitude * rb.mass);
        }

        if (Input.GetKey(KeyCode.D)) {
            rb.AddForce(-magnitude * rb.mass, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space)) {
            rb.AddForce(0, jump * rb.mass, 0);
        }

    }
}