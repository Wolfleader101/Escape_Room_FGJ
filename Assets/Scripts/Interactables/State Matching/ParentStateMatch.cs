using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script matches the state of a parent script to all current attached InteractableBase scripts on the object.
public class ParentStateMatch : MonoBehaviour {

    [SerializeField] private InteractableBase parent;

    [SerializeField] private bool invert = false;

    private InteractableBase[] interactables;

    void Start() {
        interactables = GetComponents<InteractableBase>();
    }

    void Update() {
        if(parent != null && interactables != null) {
            foreach(InteractableBase interactable in interactables) {
                if((parent.Active ^ invert) && !interactable.Active) {
                    interactable.Activate();
                } else if((!parent.Active ^ invert) && interactable.Active) {
                    interactable.Deactivate();
                }
            }
        }
    }
}
