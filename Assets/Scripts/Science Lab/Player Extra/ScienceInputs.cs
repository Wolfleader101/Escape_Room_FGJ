using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceInputs : MonoBehaviour {
    
    private PlayerInput input;

    private bool _interact = false;
    public bool Interact => _interact;

    void Awake(){
        input = new PlayerInput();
    }

    void Update() {
        _interact = input.Player.Interact.triggered;
    }

    void OnEnable(){
        input.Player.Enable();
    }

    void OnDisable(){
        input.Player.Disable();
    }
}
