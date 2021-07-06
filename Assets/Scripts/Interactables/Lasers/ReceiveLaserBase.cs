using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReceiveLaserBase : MonoBehaviour {

    public bool receivedLaser = false;

    public Vector3 hitPoint;

    private void LateUpdate() {
        receivedLaser = false;
    }

}
