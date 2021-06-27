using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScienceInputs))]
public class Interact : MonoBehaviour {
    
    // This is the maximum distance for the maximum distance an object can be for it to be interactable.
    [SerializeField] private float maxReach = 5f;

    /* Distance and size are just two settings for the jank crosshair in the middle of the screen. It just draws a tiny sphere gizmo in the centre of the screen as a crosshair.
        This is just to get an accurate idea of where I am aiming to test things. This will not be the final version of the crosshair. */
    [SerializeField] private float distance;
    [SerializeField] private float size;

    // This is a reference to the item you are currently holding.
    private HoldItem holdItem = null;

    // Reference variable for inputs.
    private ScienceInputs inputs;
    
    void Start() {
        inputs = GetComponent<ScienceInputs>();
    }

    void Update() {
        if(inputs.Interact){
            if(holdItem != null){ // If we are currently holding an object, call its Deactivate() function and set holdItem to null.
                holdItem.Deactivate();

                holdItem = null;

            } else { // Fire a ray in the direction we are looking, up to maximum reach.
                Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
                
                if(Physics.Raycast(ray, out RaycastHit hitInfo, maxReach)){
                    if(hitInfo.collider.TryGetComponent<HoldItem>(out HoldItem item)){ // If we hit a holdable item, save a reference to it and Activate() it.
                        holdItem = item;

                        holdItem.Activate();
                    } else { // For any other object:
                        InteractableBase[] interactables = hitInfo.collider.gameObject.GetComponents<InteractableBase>();
                    
                        if(interactables == null){ return; } // If interactables is null, the object does not have any class that is InteractableBase or inherited from it. Exit.

                        foreach(InteractableBase interactable in interactables){ // Call Interact() for all InteractableBase scripts on object. This is a loop as an object can have multiple interactions.
                            if(interactable.Interactable) { interactable.Interact(); } 
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos(){ // The jank crosshair.
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(Camera.main.transform.position + (Camera.main.transform.forward * distance), size);
        Gizmos.color = Color.white;
    }
}
