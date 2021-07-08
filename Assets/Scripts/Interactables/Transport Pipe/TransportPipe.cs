using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* WARNING: Make sure you set the physic material on the non-trigger collider to TransportPipe_PhysMat otherwise objects will experience too much
    friction inside of the transport tubes and may struggle to move. */

/* This script transports all Rigidbody objects inside its trigger zone along its up direction. The player will not be affected as it does
    not have a Rigidbody. */

public class TransportPipe : InteractableBase {

    [SerializeField] private float speed;

    private void OnTriggerStay(Collider collider) {
        if(_active) {
            if(collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.AddForce(transform.up * speed, ForceMode.Acceleration);
            }
        }
    }

    public override void Interact() {
        if(!_active) {
            Activate();
        } else {
            Deactivate();
        }
    }

    public override void Activate() {
        _active = true;
    }

    public override void Deactivate() {
        _active = false;
    }
}
