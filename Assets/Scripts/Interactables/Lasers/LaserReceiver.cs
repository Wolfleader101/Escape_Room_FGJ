using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReceiver : InteractableBase {

    [SerializeField] private bool canDeactivate = true;

    // Counter used to make sure the receiver can be read as active by other objects for a whole frame before deactivating.
    private int stepSinceInactive = 0;

    private void LateUpdate() {
        // Increment counter, will be reset to 0 next frame if still active. If the counter gets to 2, the receiver has been active for a full frame and can now be deactivated.
        if(canDeactivate) {
            stepSinceInactive++;
            if(stepSinceInactive >= 2) {
                Deactivate();
            }
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
