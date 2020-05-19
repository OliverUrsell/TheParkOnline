using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BlockPlayer : NetworkBehaviour {

    public Rigidbody rb;
    public float movementAccerleration = 15F;
    public float maxMovementSpeed = 20F;
    public float rotationAcceleration = 0.2F;
    public float maxRotation = 2F;
    public float jump = 6F;
    public float jetpackPower = 10f;
    public ParticleSystem jetpackParticles;
    private ParticleSystem.EmissionModule emission; //The specific emission module of the particles
    private Rigidbody vehicleRB; //Keeps track of the rb of the vehicle we are currently on
    private float raycastDistance;
    public float jumpingGroundDistance = 0.1F;
    public bool localPlayer = false;

    public bool debug = false;
    public bool qeRotation = false;

    void Start() {
        localPlayer = this.isLocalPlayer;
        if (localPlayer) {
            raycastDistance = GetComponent<Collider>().bounds.extents.y + jumpingGroundDistance;        
        }
        emission = jetpackParticles.emission;
    }

    void FixedUpdate() {
        //coding our own drag so it acts locally, and doesn't kick us off vehicles
        Vector3 relativeVelocity;
        float y = rb.velocity.y; //Stop the drag effecting the y axis
        try {
            vehicleRB = transform.parent.parent.parent.GetComponent<Rigidbody>(); //We need to go three parents up as this script is on the second child of the thing that is parented to the vehicle
            relativeVelocity = rb.velocity - vehicleRB.velocity;
            if (relativeVelocity != Vector3.zero) {
                //reduce our velocity by some factor
                rb.velocity = relativeVelocity * 9 / 10 + vehicleRB.velocity;
            }
        } catch {
            //We aren't on a vehicle so calculate drag like normal, ie with realtive velocity as just our normal velocity
            relativeVelocity = rb.velocity;
            if (relativeVelocity != Vector3.zero) {
                //reduce our velocity by some factor
                rb.velocity = relativeVelocity * 14 / 15;
            }
        }
        rb.velocity = new Vector3(rb.velocity.x, y, rb.velocity.z); //reset our y velocity

        if (localPlayer) {

            if (Input.GetKey(KeyCode.W) & rb.velocity.magnitude < maxMovementSpeed) {
                rb.AddForce(movementAccerleration * rb.mass * transform.forward);
            }

            if (Input.GetKey(KeyCode.A) & rb.velocity.magnitude < maxMovementSpeed) {
                rb.AddForce(movementAccerleration * rb.mass * -transform.right);
            }

            if (Input.GetKey(KeyCode.S) & rb.velocity.magnitude < maxMovementSpeed) {
                rb.AddForce(movementAccerleration * rb.mass * -transform.forward);
            }

            if (Input.GetKey(KeyCode.D) & rb.velocity.magnitude < maxMovementSpeed) {
                rb.AddForce(movementAccerleration * rb.mass * transform.right);
            }
            /*if(Input.GetAxis("Horizontal") == 0 & Input.GetAxis("Vertical") == 0) {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }*/
            if (qeRotation) {
                if (Input.GetKey(KeyCode.Q) & rb.rotation.y > -maxRotation) {
                    rb.AddTorque(0, -rotationAcceleration, 0, ForceMode.VelocityChange);
                }
                if (Input.GetKey(KeyCode.E) & rb.rotation.y < maxRotation) {
                    rb.AddTorque(0, rotationAcceleration, 0, ForceMode.VelocityChange);
                }
                if (!(Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Q))) {
                    rb.angularVelocity = Vector3.zero;
                }
            }

            //This stops all non-vertical movement when the movement keys aren't held down
            /*if(!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
            }*/


            if (Input.GetKey(KeyCode.Space)) {
                if (Physics.Raycast(transform.position, -Vector3.up, raycastDistance)) {
                    rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
                }
            }

            if (debug) {
                Debug.DrawRay(transform.position, Vector3.down * raycastDistance, new Color(255, 0, 0));
            }

            //Jetpack code
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                //When either shift key is pressed, add an upwards force turn on the jetpack particles
                rb.AddForce(Vector3.up * jetpackPower, ForceMode.Acceleration);

                emission.enabled = true;
            } else if (emission.enabled) {
                //If the jetpack has just been turned off
                emission.enabled = false; //disable the jetpack particles
            }
        } else {
            emission.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Vehicle") { //Checking for trigger allows us to select which sections do and don't count as getting on the vehicle
            //set our parent to a vehicle if we land on it
            transform.parent.parent.SetParent(other.transform, true);
        }else{
            //if we collide with something that isn't a vehicle set our parent to null
            //Only do this when we impact something new so that we can still jump on board a vehicle without drag
            transform.parent = null;
        }
    }
}
