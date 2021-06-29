using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an abstract class. It is the base class that all interactable items should inherit from. Do not attach this script to an object.
public abstract class InteractableBase : MonoBehaviour {

    // This variable defines whether the player is able to call the object's Interact() function.
    [SerializeField] protected bool _interactable = true;
    public bool Interactable => _interactable;

    // This defines the current state of the variable, whether it is in active or inactive mode.
    protected bool _active = false;
    public bool Active => _active;

    /*  These are the three functions that need to be implemented in all classes that inherit from this class.
        To reimplement Interact(), for example, you need to type 'public override void Interact(){...}'. 
        The override keyword lets you reimplement the function. */
    public abstract void Interact();
    public abstract void Activate();
    public abstract void Deactivate();
}
