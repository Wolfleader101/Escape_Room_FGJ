using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is similar to Interact, however it involves interacting with the specified interactables through colliding with them.
public class CollisionInteract : MonoBehaviour {

    // This is the list of all interactions compatible with CollisionInteract. Select which interactions are valid for collisions.
    [Header("Interactable Settings")]
    [SerializeField] private bool button = false;
    [SerializeField] private bool movingPlatform = false;
    [SerializeField] private bool switchPositions = false;
    [SerializeField] private bool switchMaterials = false;

    // Customisations for certain interaction categories.
    [Header("Customisation")]
    [SerializeField] private bool buttonTypeMustCollideTop = true;

    private void OnCollisionEnter(Collision collisionInfo) {
        GameObject obj = collisionInfo.collider.gameObject;

        InteractableBase[] interactables = obj.GetComponents<InteractableBase>();

        // If interactables is null, obj is not an interactable. Exit.
        if(interactables == null) { return; }

        /* If button collisions do not need to be from the top, var will be set to true and the next if statement will be skipped.
            Otherwise, var will be false, and validity will be determined inside the if statement. */
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

        // Check if object is a button type interaction.
        bool isButtonType = false;
        foreach(InteractableBase interactable in interactables) {
            isButtonType |= (interactable is Button);
            if(interactable is Button) {
                print("Found Button");
            }
        }

        // If object is a button type, check whether interacting with the object is a valid option. If so, call Interact() on all interactables.
        if(isButtonType) {
            bool interact = buttonTypeInteractValid && (button);

            if(interact) {
                foreach(InteractableBase interactable in interactables) {
                    interactable.Interact();
                }
            }
        } else {
            // Loop through all interactables on colliding object, and if it is a valid collision interaction type, call Interact().
            foreach(InteractableBase interactable in interactables) {
                // If interactable is not a button type, test for all other potential interactions.
                bool interact = (movingPlatform && interactable is MovingPlatform) ||
                                (switchPositions && interactable is SwitchPositions) ||
                                (switchMaterials && interactable is SwitchMaterials);

                // If valid, Interact().
                if(interact) {
                    interactable.Interact();
                }
            }
        }
    }
}
