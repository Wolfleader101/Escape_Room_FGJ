using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : InteractableBase {

    [HideInInspector] public Vector3 hitPoint;

    [HideInInspector] public Vector3 hitNormal;

    [HideInInspector] public float damagePerSec;

    [HideInInspector] public bool damagePlayerOnly;

    [HideInInspector] public Vector3 incomingDir;

    [HideInInspector] public int cutoff;

    private int stepSinceInactive = 0;

    private void LateUpdate() {
        stepSinceInactive++;
        if(stepSinceInactive >= 2) {
            Deactivate();
        }
    }

    public override void Interact() {
        return;
    }

    public override void Activate() {
        _active = true;
        stepSinceInactive = 0;
    }

    public override void Deactivate() {
        _active = false;
    }
}
