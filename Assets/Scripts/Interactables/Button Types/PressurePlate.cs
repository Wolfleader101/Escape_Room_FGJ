using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PressurePlate : InteractableBase {

    private int collisionCount = 0;

    // Pressure plates must use CollisionInteract to allow interactions with itself.
    private void OnValidate() {
        _interactable = false;
    }

    private void Start() {
        // Set specific Rigidbody settings explicitly.
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = true;
    }

    private void Update() {
        // The pressure plates must Deactivate() itself, as CollideInteract only interacts with objects through OnCollisionEnter/OnControllerColliderHit(), not also on exit.
        if(_active && collisionCount <= 0) {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if(!collider.gameObject.isStatic) {
            collisionCount++;
        }

    }

    private void OnTriggerExit(Collider collider) {
        if(!collider.gameObject.isStatic) {
            collisionCount--;
        }
    }

    public override void Interact() {
        if(!_active && collisionCount > 0) {
            Activate();
        } else if(_active && collisionCount <= 0) {
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
