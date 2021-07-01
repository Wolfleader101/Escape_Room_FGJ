using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportPipe : InteractableBase {

    [SerializeField] private float speed;

    private void OnTriggerStay(Collider collider) {
        if(_active) {
            if(collider.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                //rb.MovePosition(rb.position + (transform.up * speed * Time.deltaTime));
                rb.AddForce(transform.up * speed, ForceMode.Acceleration);
            }
        }
    }

    public override void Interact() {
        if(!_active) {
            Activate();
        } else {
            Deactivate();
        }
    }

    public override void Activate() {
        _active = true;
    }

    public override void Deactivate() {
        _active = false;
    }
}
