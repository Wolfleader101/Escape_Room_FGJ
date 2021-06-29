using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    // the player input component
    [SerializeField] private PlayerInput input;

    // This is the maximum distance for the maximum distance an object can be for it to be interactable.
    [SerializeField] private float maxReach = 5f;

    /* Distance and size are just two settings for the jank crosshair in the middle of the screen.
       It just draws a tiny sphere gizmo in the centre of the screen as a crosshair.
       This is just to get an accurate idea of where I am aiming to test things. This will not be the final version of the crosshair. */
    [SerializeField] private float distance = 0.4f;
    [SerializeField] private float size = 0.0025f;

    // This is a reference to the item you are currently holding.
    private HoldItem holdItem = null;

    // is interacting with object
    private bool _isInteracting;

    private void Start() {
        if(input == null){ 
            input = GetComponent<PlayerInput>();
        }
    }

    void Update()
    {
        if (_isInteracting)
        {
            // If we are currently holding an object, call its Deactivate() function and set holdItem to null and return
            if (holdItem != null)
            {
                holdItem.Deactivate();
                holdItem = null;

                return;
            }

            // Fire a ray in the direction we are looking, up to maximum reach.
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

            if (Physics.Raycast(ray, out RaycastHit hitInfo, maxReach))
            {
                // If we hit a holdable item, save a reference to it and Activate() it.
                if (hitInfo.collider.TryGetComponent<HoldItem>(out HoldItem item))
                {
                    holdItem = item;
                    holdItem.Activate();

                    return;
                }

                // For any other object:
                InteractableBase[] interactables = hitInfo.collider.gameObject.GetComponents<InteractableBase>();

                // If interactables is null, the object does not have any class that is InteractableBase or inherited from it. Exit.
                if (interactables == null) return;
                
                foreach (InteractableBase interactable in interactables)
                {
                    // Call Interact() for all InteractableBase scripts on object. This is a loop as an object can have multiple interactions.
                    if (interactable.Interactable) interactable.Interact();
                }
            }
        }
    }

    private void LateUpdate(){
        _isInteracting = false;
    }

    private void OnDrawGizmos()
    {
        // store main cam as a variable as calling Camera.main is expensive
        var cam = Camera.main;

        if (cam == null) return;
        
        // The jank crosshair.
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(cam.transform.position + (cam.transform.forward * distance), size);
        Gizmos.color = Color.white;
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        _isInteracting = value.canceled;
    }
}