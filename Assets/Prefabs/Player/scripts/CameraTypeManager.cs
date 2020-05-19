using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraTypeManager : NetworkBehaviour {

    public enum ControlType {
        ThirdPerson,
        FirstPerson
    }
    private ControlType controlType = ControlType.FirstPerson;

    public GameObject firstPerson;
    public GameObject thirdPerson;

    public ControlType[] cameraOrder = {ControlType.ThirdPerson, ControlType.FirstPerson};
    private int cameraOrderPosition = 0;

    void Start() {
        if (this.isLocalPlayer) {
            setCameraType(cameraOrder[0]);
        } else {
            turnOffAll();
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(2) && this.isLocalPlayer) {
            cameraOrderPosition++;
            if(cameraOrderPosition == cameraOrder.Length) {
                cameraOrderPosition = 0;
            }
            setCameraType(cameraOrder[cameraOrderPosition]);
        }
    }

    void setCameraType(ControlType ct) {
        switch (ct) {
            case ControlType.FirstPerson:
                firstPerson.GetComponent<Camera>().enabled = true;
                firstPerson.transform.GetChild(0).gameObject.SetActive(true);
                firstPerson.transform.localRotation =  thirdPerson.transform.localRotation;
                thirdPerson.GetComponent<Camera>().enabled = false;
                break;
            case ControlType.ThirdPerson:
                thirdPerson.GetComponent<Camera>().enabled = true;
                firstPerson.transform.GetChild(0).gameObject.SetActive(false);
                thirdPerson.transform.localRotation = firstPerson.transform.localRotation;
                firstPerson.GetComponent<Camera>().enabled = false;
                break;
        }
        controlType = ct;
    }

    public void turnOffAll() {
        firstPerson.SetActive(false);
        thirdPerson.SetActive(false);
    }

    public void turnOn() {
        firstPerson.SetActive(true);
        thirdPerson.SetActive(true);
    }
}
