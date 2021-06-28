using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  This is a button-type interactable. It can be either toggleable or single use.
    The button will make all of its linked interactable objects match its current state.
    This means that if a linked object is already active, and you activate the button, 
    the linked object will stay active. The button does not just do a simple toggle
    of all linked objects' states. */
public class Button : InteractableBase {
    
    // Boolean field for whether the object can only be used once or pressed repeatedly.
    [SerializeField] private bool isSingleUse = false;
    
    // List of objects this interactable will Activate() or Deactivate().
    [SerializeField] private InteractableBase[] objects;

    public override void Interact()
    {
        if(!_active){
            Activate();
        } else if(!isSingleUse) { // Will only allow deactivation if button is toggleable.
            Deactivate();
        }
    }

    public override void Activate(){
        _active = true;

        foreach(InteractableBase obj in objects){ // Activate() all linked objects.
            obj.Activate();
        }
    }

    public override void Deactivate(){ // Deactivate() all linked objects.
        _active = false;

        foreach(InteractableBase obj in objects){
            obj.Deactivate();
        }
    }
}
