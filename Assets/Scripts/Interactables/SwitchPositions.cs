using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPositions : InteractableBase {

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
        if(_active || (!_active && canDeactivate)) {
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
        if(!_active) {
            Activate();
        } else if(canDeactivate) {
            Deactivate();
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

    [ContextMenu("Set Inactive Pos to Obj Pos")]
    private void SetInactiveToObjPos() {
        inactivePos = transform.position;
    }
}
