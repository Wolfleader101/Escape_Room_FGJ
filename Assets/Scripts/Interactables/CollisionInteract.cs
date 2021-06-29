using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is similar to Interact, however it involves interacting with the specified interactables through colliding with them.
public class CollisionInteract : MonoBehaviour {

    // This is the list of all interactions compatible with CollisionInteract. Select which interactions are valid for collisions.
    [Header("Interactable Settings")]
    [SerializeField] private bool button = false;
    [SerializeField] private bool pressurePlate = false;
    [Header("")]
    [SerializeField] private bool movingPlatform = false;
    [SerializeField] private bool switchPositions = false;
    [SerializeField] private bool switchMaterials = false;

    // Customisations for certain interaction categories.
    [Header("Customisation")]
    // Whether a collision with a button type interaction must occur from generally the top to be valid. Top is defined as the button's transform.up.
    [SerializeField] private bool buttonTypeMustCollideTop = true;

    private void OnCollisionEnter(Collision collisionInfo) {
        GameObject obj = collisionInfo.collider.gameObject;

        InteractableBase[] interactables = obj.GetComponents<InteractableBase>();

        // If interactables is null, obj is not an interactable. Exit.
        if(interactables == null) { return; }

        if(obj.TryGetComponent<PressurePlate>(out PressurePlate plate)) {
            print("Pressure Plate");
        }

        /* If button collisions do not need to be from the top, var will be set to true and the next if statement will be skipped.
            Otherwise, var will start as false, and validity will be determined inside the if statement. */
        bool buttonTypeInteractValid = !buttonTypeMustCollideTop;

        if(buttonTypeMustCollideTop) {
            ContactPoint[] points = new ContactPoint[collisionInfo.contactCount];
            collisionInfo.GetContacts(points);

            foreach(ContactPoint point in points) {
                // If any collision point is generally from the top, set button types as a valid interaction.
                if(Vector3.Dot(point.normal, obj.transform.up) > 0.2f) {
                    buttonTypeInteractValid = true;
                    break;
                }
            }
        }

        // If any of the valid interactables is found, run Interact() on all interactables on the colliding object.
        bool valid = false;
        foreach(InteractableBase interactable in interactables) {
            valid |= buttonTypeInteractValid &&
                     (button && interactable is Button) ||
                     (pressurePlate && interactable is PressurePlate);

            valid |= (movingPlatform && interactable is MovingPlatform) ||
                     (switchPositions && interactable is SwitchPositions) ||
                     (switchMaterials && interactable is SwitchMaterials);

            if(valid) {
                foreach(InteractableBase _interactable in interactables) {
                    _interactable.Interact();
                }

                break;
            }
        }
    }
}
