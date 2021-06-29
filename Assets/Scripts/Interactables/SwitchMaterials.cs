using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple interaction, mainly for test purposes, that simply toggles an objects materials between two
public class SwitchMaterials : InteractableBase {

    // Whether the interaction is toggleable or single use.
    [SerializeField] private bool reversible = true;

    // Inactive material.
    [SerializeField] private Material mat1;

    // Active material.
    [SerializeField] private Material mat2;

    private Renderer rend;

    // This function activates whenever you edit something in the inspector. It will set the object's material to its inactive one (mat1), if possible.
    void OnValidate() {
        if(mat1 != null) {
            GetComponent<Renderer>().sharedMaterial = mat1;
        }
    }

    private void Start() {
        rend = GetComponent<Renderer>();
    }

    public override void Interact() {
        if(!_active) {
            Activate();
        } else if(reversible) {
            // Will only swap back to mat1 if it is reversible.
            Deactivate();
        }
    }

    public override void Activate() {
        _active = true;
        rend.sharedMaterial = mat2;
    }

    public override void Deactivate() {
        _active = false;
        rend.sharedMaterial = mat1;
    }
}
