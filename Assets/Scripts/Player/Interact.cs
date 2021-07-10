using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour {
    // the player input component
    [SerializeField] private PlayerInput input;

    // This is the maximum distance an object can be before it becomes out of reach.
    [SerializeField] private float maxReach = 5f;

    // The speed of rotating a holdable item.
    [SerializeField] private float rotateSpeed = 100f;

    // The speed of changing a holdable item's distance point.
    [SerializeField] private float distChangeSpeed = 1f;

    // This is a reference to the item you are currently holding.
    [HideInInspector] public HoldItem holdItem = null;

    // is interacting with object
    private bool _isInteracting;

    // Is rotating hold item (if currently holding one)
    private bool _isRotatingItem;
    private float _rotateDir;

    // Is changing hold item distance (if currently holding one)
    private bool _isDistChangeItem;
    private float _distanceAdjust;

    private void Start() {
        if(input == null) {
            input = GetComponent<PlayerInput>();
        }
    }

    void Update() {
        if(holdItem != null) {
            if(_isRotatingItem) { holdItem.Rotate(_rotateDir * rotateSpeed); }
            if(_isDistChangeItem) { holdItem.AdjustHoldDistance(_distanceAdjust * distChangeSpeed); }
        }

        if(_isInteracting) {
            // If we are currently holding an object, call its Deactivate() function and set holdItem to null and return
            if(holdItem != null) {
                holdItem.Deactivate();
                holdItem = null;

                return;
            }

            // Fire a ray in the direction we are looking, up to maximum reach.
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));

            if(Physics.Raycast(ray, out RaycastHit hitInfo, maxReach, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.Ignore)) {
                // If we hit a holdable item, save a reference to it and Activate() it.
                if(hitInfo.collider.TryGetComponent<HoldItem>(out HoldItem item)) {
                    holdItem = item;
                    holdItem.Activate();

                    return;
                }

                // For any other object:
                InteractableBase[] interactables = hitInfo.collider.gameObject.GetComponents<InteractableBase>();

                // If interactables is null, the object does not have any class that is InteractableBase or inherited from it. Exit.
                if(interactables == null) return;

                foreach(InteractableBase interactable in interactables) {
                    // Call Interact() for all InteractableBase scripts on object. This is a loop as an object can have multiple interactions.
                    if(interactable.Interactable) interactable.Interact();
                }
            }
        }
    }

    private void LateUpdate() {
        _isInteracting = false;
    }

    public void OnInteract(InputAction.CallbackContext value) {
        _isInteracting = value.canceled;
    }

    public void OnRotateItem(InputAction.CallbackContext value) {
        _isRotatingItem = value.performed;
        _rotateDir = value.ReadValue<float>();
    }

    public void OnDistanceChangeItem(InputAction.CallbackContext value) {
        _isDistChangeItem = value.performed;
        _distanceAdjust = value.ReadValue<float>();
    }
}