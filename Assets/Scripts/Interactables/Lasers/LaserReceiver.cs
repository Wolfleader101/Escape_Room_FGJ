using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : InteractableBase {

    [HideInInspector] public Vector3 hitPoint;

    [HideInInspector] public float damagePerSec;

    [HideInInspector] public bool damagePlayerOnly;

    private void LateUpdate() {
        Deactivate();
    }

    public override void Interact() {
        return;
    }

    public override void Activate() {
        _active = true;
    }

    public override void Deactivate() {
        _active = false;
    }
}
