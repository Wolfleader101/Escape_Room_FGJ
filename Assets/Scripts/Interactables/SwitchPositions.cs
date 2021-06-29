using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPositions : InteractableBase {

    /* This is the parent interaction of this interaction. If you set a parent, it will match its active state with its parent.
        If no parent is set, this interaction will act like a regular interaction. */
    [SerializeField] private InteractableBase parent;

    [SerializeField] private bool canDeactivate;
    [SerializeField] private bool instantDeactivate;

    [SerializeField] private Vector3 activeOffset;

    [SerializeField] private float speed;

    [SerializeField] private bool usePhysics;

    private Vector3 inactivePos;

    private Vector3 activePos;

    private Vector3 targetPos;

    private Rigidbody rb;

    private void OnValidate() {
        if(!canDeactivate) {
            instantDeactivate = false;
        }
    }

    private void Start() {
        inactivePos = transform.position;
        activePos = transform.position + activeOffset;
        targetPos = inactivePos;

        if(usePhysics) {
            rb = GetComponent<Rigidbody>();
            if(rb == null) { usePhysics = false; return; }
            rb.isKinematic = true;
        }
    }

    private void Update() {
        if(!usePhysics) {
            MovePosition();
        }
    }

    private void FixedUpdate() {
        if(usePhysics) {
            MovePosition();
        }
    }

    private void MovePosition() {
        if(parent != null) {
            if(_active && !parent.Active) {
                Deactivate();
            }
        }

        if(_active || canDeactivate) {
            if(transform.position != targetPos) {
                Vector3 dir = (targetPos - transform.position);

                if(dir.magnitude <= (speed * Time.deltaTime)) {
                    if(usePhysics) {
                        rb.MovePosition(targetPos);
                    } else {
                        transform.position = targetPos;
                    }

                    if(canDeactivate && instantDeactivate) {
                        Deactivate();
                    }

                } else {
                    if(usePhysics) {
                        rb.MovePosition(transform.position + (dir.normalized * speed * Time.deltaTime));
                    } else {
                        transform.position += dir.normalized * speed * Time.deltaTime;
                    }
                }
            }
        }
    }

    public override void Interact() {
        if(parent == null) {
            if(!_active) {
                Activate();
            } else if(canDeactivate) {
                Deactivate();
            }
        } else {
            if(!_active && parent.Active) {
                Activate();
            } else if(_active && !parent.Active) {
                Deactivate();
            }
        }
    }

    public override void Activate() {
        _active = true;
        targetPos = activePos;
    }

    public override void Deactivate() {
        _active = false;
        targetPos = inactivePos;
    }
}
