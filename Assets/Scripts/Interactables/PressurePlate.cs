using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PressurePlate : InteractableBase {

    /* WARNING: if you are having problems with using the pressure plate, there are two things you can do. First, turn debug view on in the inspector and see the collisionCount value
        when the game starts, if this number is any amount other than 0, then set the negative of that number as the collisionCountOffset. If the platform works with holdable items
        but not the player, make sure that the trigger zone is sufficiently large for the player. I have found that with a cube vertically scaled to 0.2, the trigger zone needed to be
        at least size 3.5 vertically for the pressure plate to operate smoothly. */

    [SerializeField] private int collisionCountOffset = 0;

    /* This number is set to -2 as the pressure plate will always collide with the ground and the base plate part of its frame.
        Setting this to -2 effectively negates these two colliders from interacting with the pressure plate. */
    private int collisionCount = 0;

    // Pressure plates must use CollisionInteract to allow interactions with itself.
    private void OnValidate() {
        _interactable = false;
    }

    private void Start() {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = true;

        collisionCount = collisionCountOffset;
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
