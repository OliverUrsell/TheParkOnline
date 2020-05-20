

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    // Update is called once per frame
    void Update(){
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, Camera.current.transform.position - transform.position, 360,360);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
