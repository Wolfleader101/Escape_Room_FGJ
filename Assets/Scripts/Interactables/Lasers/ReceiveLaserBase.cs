using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReceiveLaserBase : InteractableBase {

    [HideInInspector] public bool receivedLaser = false;

    [HideInInspector] public Vector3 hitPoint;

    private void LateUpdate() {
        Deactivate();
    }

    public override void Interact() {
        return;
    }

    public override void Activate() {
        _active = true;
        receivedLaser = true;
    }

    public override void Deactivate() {
        _active = false;
        receivedLaser = false;
    }
}
