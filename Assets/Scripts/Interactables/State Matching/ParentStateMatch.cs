using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script matches the state of a parent script to all current attached InteractableBase scripts on the object.
public class ParentStateMatch : MonoBehaviour {

    [SerializeField] private InteractableBase parent;

    private InteractableBase[] interactables;

    void Start() {
        interactables = GetComponents<InteractableBase>();
    }

    void Update() {
        if(interactables != null) {
            foreach(InteractableBase interactable in interactables) {
                if(parent.Active && !interactable.Active) {
                    interactable.Activate();
                } else if(!parent.Active && interactable.Active) {
                    interactable.Deactivate();
                }
            }
        }
    }
}
