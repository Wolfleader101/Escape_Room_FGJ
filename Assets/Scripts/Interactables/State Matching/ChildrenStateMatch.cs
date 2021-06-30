using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* WARNING: This script is the less efficient version of ParentStateMatch. This script should only be used on a parent object if it needs to state match MANY children to itself.
    If there is only one or a few children that need to state match with a parent object, it is more efficient to use ParentStateMatch on each individual child object. */

// This script is used on a parent object to have all children state match their scripts with the reference script.
public class ChildrenStateMatch : MonoBehaviour {

    // The reference interactable to state match all children to. Usually the main interactable on the object this script is attached to.
    [SerializeField] private InteractableBase reference;

    // A list of all children objects to state match to this object.
    [SerializeField] private GameObject[] children;

    void Update() {
        if(reference != null && children != null) {
            foreach(GameObject child in children) {
                foreach(InteractableBase interactable in child.GetComponents<InteractableBase>()) {
                    if(reference.Active && !interactable.Active) {
                        interactable.Activate();
                    } else if(!reference.Active && interactable.Active) {

                    }
                }
            }
        }
    }
}
