using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PressurePlate : InteractableBase {

    /* This number is set to -2 as the pressure plate will always collide with the ground and the base plate part of its frame.
        Setting this to -2 effectively negates these two colliders from interacting with the pressure plate. */
    private int collisionCount = -2;

    // Pressure plates must use CollisionInteract to allow interactions with itself.
    private void OnValidate() {
        _interactable = false;
    }

    private void Start() {
        GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update() {
        if(_active && collisionCount <= 0) {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider collider) {
        collisionCount++;
    }

    private void OnTriggerExit(Collider collider) {
        collisionCount--;
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
