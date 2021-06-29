using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PressurePlate : InteractableBase {

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
        if(collider.gameObject.TryGetComponent<FirstPersonController>(out FirstPersonController fpc)) {
            print("PLayer Found");
        }
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
