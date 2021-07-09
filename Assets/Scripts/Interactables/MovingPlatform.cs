using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : InteractableBase {

    // Whether the object will continue looping through the positions, repeating back to the first index when at the end.
    [SerializeField] private bool repeatable;

    // Whether the object will autonomously travel between positions continuously, without need for input.
    [SerializeField] private bool continuous;

    [SerializeField] private float speed;

    [SerializeField] private float waitTime;

    [SerializeField] private int index = 0;

    [SerializeField] Vector3[] positions;

    private Rigidbody rb;

    private int direction = 1;

    private Vector3 oldPos;

    private float timer;

    private Vector3 startPos;
    private float percentTravelled;

    private void OnValidate() {
        index = Mathf.Clamp(index, 0, Mathf.Max(positions.Length - 1, 0));
    }

    private void Start() {
        // Set specific Rigidbody settings explicitly.
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = true;
    }

    private void FixedUpdate() {
        if(_active) {
            // If timer is active (is not 0), reduce timer value and do not run any extra code.
            if(timer > 0f) {
                timer -= Time.deltaTime;
                return;
            }

            // If index position is greater than accessible points and is travelling forward:
            if(index >= positions.Length && direction == 1) {
                if(repeatable) {
                    // Loop index back to start.
                    index = 0;
                    startPos = transform.position;
                } else {
                    // Reverse travel direction and set index to previous point.
                    direction = -1;
                    index = Mathf.Max(positions.Length - 2, 0);
                }
            } else if(index <= 0 && direction == -1) {
                // If travelling in reverse and we have hit the start, set direction to forward again and we will move that direction on next index change.
                direction = 1;
            }

            // Calculate the travel direction to next point. The point is not normalised so we can also get the distance to the next point (dir.magnitude).
            Vector3 dir = (positions[index] - transform.position);

            // If the distance we want to travel will overshoot the point, simply just set the platform's position to the point.
            if(dir.magnitude <= (speed * Time.deltaTime)) {
                rb.MovePosition(positions[index]);
                index += direction;
                timer = waitTime;
                Deactivate();
            } else {
                // Move in the direction of the point, at the speed of 'speed'.
                rb.MovePosition(transform.position + (dir.normalized * speed * Time.deltaTime));
            }
        } else if(continuous) {
            // This essentially reactivates the platform, as long as the timer is at 0.
            Interact();
        }

        oldPos = transform.position;
    }

    private void OnTriggerStay(Collider collider) {
        GameObject obj = collider.gameObject;

        // This moves objects on the platform, using their respective correct move functions.
        if(obj.TryGetComponent<CharacterController>(out CharacterController ccOther)) {
            ccOther.Move(rb.velocity * Time.deltaTime);
        } else if(obj.TryGetComponent<Rigidbody>(out Rigidbody rbOther)) {
            if(rb.velocity != Vector3.zero) {
                rbOther.velocity = new Vector3(rb.velocity.x, rbOther.velocity.y, rb.velocity.z);
            }
        }
    }

    public override void Interact() {
        if(!_active && timer <= 0f) {
            Activate();
        }
    }

    public override void Activate() {
        _active = true;
    }

    public override void Deactivate() {
        _active = false;
    }

    /* The two ContextMenu function below are helper function for more easily creating the waypoints for the platform. To use each function, set the index variable
        in the inspector to the index of the position you want to work with. On the moving platform script, click the three dots icon and at the bottom you will see
        the two ContextMenu functions. */

    // The first sets the objects current position to the position specified at the index value you selected.
    [ContextMenu("Set Position at Index to Obj Position")]
    private void SetPosToObjPos() {
        if(positions.Length == 0) { return; }

        positions[index] = transform.position;
    }

    // It sets the position at the specified index to be the objects position. This is useful for putting the object back to the first position in the array.
    [ContextMenu("Set Obj Position to Position at Index")]
    private void SetObjPosToPos() {
        if(positions.Length == 0) { return; }

        transform.position = positions[index];
    }

    // This function draws the Gizmo points and lines to help visualise the path of the platform.
    private void OnDrawGizmosSelected() {
        if(positions.Length == 0) { return; }

        for(int i = 0; i < positions.Length; i++) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(positions[i], 0.2f);

            if(i == positions.Length - 1) {
                if(repeatable) {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(positions[i], positions[0]);
                }
            } else {
                Gizmos.color = Color.red;

                Gizmos.DrawLine(positions[i], positions[i + 1]);
            }
        }

        Gizmos.color = Color.white;
    }
}
