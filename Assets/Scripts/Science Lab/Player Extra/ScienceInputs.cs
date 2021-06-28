using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceInputs : MonoBehaviour {
    
    // Reference variable for inputs.
    private PlayerInput input;

    // Input variables that are set and can be accessed by other classes.
    private bool _interact = false;
    public bool Interact => _interact;

    void Awake(){
        input = new PlayerInput();
    }

    void Update() { // Set state of all inputs.
        _interact = input.Player.Interact.triggered;
    }

    void OnEnable(){
        input.Player.Enable();
    }

    void OnDisable(){
        input.Player.Disable();
    }
}
