using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour {
    
    protected bool _active = false;
    public bool Active => _active;

    public abstract void Interact();
    public abstract void Activate();
    public abstract void Deactivate();
}
