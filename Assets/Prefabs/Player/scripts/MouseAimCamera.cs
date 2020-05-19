using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimCamera : MonoBehaviour {

    public Transform CameraTarget;
    private float x = 0.0f;
    private float y = 0.0f;

    private int mouseXSpeedMod = 5;
    private int mouseYSpeedMod = 5;

    public float MaxViewDistance = 15f;
    public float MinViewDistance = 1f;
    public int ZoomRate = 20;
    [SerializeField] private float distance = 6f;
    private float desireDistance;
    private float correctedDistance;
    private float currentDistance;

    public float cameraTargetHeight = 1.0f;
    public float maxAngle = 80;
    public float minAngle = 10;
    public int lerpRate = 5;
    public bool useMouseHold = true;
    public bool invertVerticalMouseLook = true;

    private bool localPlayer;

    // Use this for initialization
    void Start() {
        localPlayer = transform.parent.GetComponent<CameraTypeManager>().isLocalPlayer;
        if (localPlayer) {
            Vector3 Angles = transform.eulerAngles;
            x = Angles.x;
            y = Angles.y;
            currentDistance = distance;
            desireDistance = distance;
            correctedDistance = distance;

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (localPlayer) {
            if (Input.GetMouseButton(1) || !useMouseHold) {/*0 mouse btn izq, 1 mouse btn der*/
                x += Input.GetAxis("Mouse X") * mouseXSpeedMod;
                if (invertVerticalMouseLook) {
                    y += Input.GetAxis("Mouse Y") * mouseYSpeedMod;
                } else {
                    y -= Input.GetAxis("Mouse Y") * mouseYSpeedMod;
                }
            } else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) {
                float targetRotantionAngle = CameraTarget.eulerAngles.y;
                float cameraRotationAngle = transform.eulerAngles.y;
                x = Mathf.LerpAngle(cameraRotationAngle, targetRotantionAngle, lerpRate * Time.deltaTime);
            }

            y = ClampAngle(y, minAngle, maxAngle);
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            desireDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomRate * Mathf.Abs(desireDistance);
            desireDistance = Mathf.Clamp(desireDistance, MinViewDistance, MaxViewDistance);
            correctedDistance = desireDistance;

            Vector3 position = CameraTarget.position - (rotation * Vector3.forward * desireDistance);

            RaycastHit collisionHit;
            Vector3 cameraTargetPosition = new Vector3(CameraTarget.position.x, CameraTarget.position.y + cameraTargetHeight, CameraTarget.position.z);

            bool isCorrected = false;
            if (Physics.Linecast(cameraTargetPosition, position, out collisionHit)) {
                position = collisionHit.point;
                correctedDistance = Vector3.Distance(cameraTargetPosition, position);
                isCorrected = true;
            }

            //?
            //condicion ? first_expresion : second_expresion;
            //(input > 0) ? isPositive : isNegative;

            currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * ZoomRate) : correctedDistance;

            position = CameraTarget.position - (rotation * Vector3.forward * currentDistance + new Vector3(0, -cameraTargetHeight, 0));

            transform.rotation = rotation;
            transform.position = position;

            //CameraTarget.rotation = rotation;

            float cameraX = transform.rotation.x;
            //checks if right mouse button is pushed
            if (Input.GetMouseButton(1) || !useMouseHold) {
                //sets CHARACTERS x rotation to match cameras x rotation
                CameraTarget.eulerAngles = new Vector3(cameraX, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
    }

    private static float ClampAngle(float angle, float min, float max) {
        if (angle < -360) {
            angle += 360;
        }
        if (angle > 360) {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}