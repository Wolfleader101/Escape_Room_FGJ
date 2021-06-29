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

    private Vector3 inactivePos;

    private Vector3 activePos;

    private Vector3 targetPos;

    private void OnValidate() {
        if(!canDeactivate) {
            instantDeactivate = false;
        }
    }

    private void Start() {
        inactivePos = transform.position;
        activePos = transform.position + activeOffset;
        targetPos = inactivePos;
    }

    private void Update() {
        if(parent != null) {
            if(_active && !parent.Active) {
                Deactivate();
            }
        }

        if(_active || canDeactivate) {
            if(transform.position != targetPos) {
                Vector3 dir = (targetPos - transform.position);

                if(dir.magnitude <= (speed * Time.deltaTime)) {
                    transform.position = targetPos;
                    if(canDeactivate && instantDeactivate) {
                        Deactivate();
                    }
                } else {
                    transform.position += dir.normalized * speed * Time.deltaTime;
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
