using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : InteractableBase {

    [SerializeField] private bool startActive = true;

    [SerializeField] private bool toggleable = false;

    [SerializeField] private bool canKillPlayer = true;

    private void OnValidate() {
        _interactable = false;
    }

    private void Start() {
        _active = startActive;
    }

    private void OnTriggerStay(Collider collider) {
        if(_active) {
            if(collider.gameObject.TryGetComponent<Health>(out Health health)) {
                if(health.IsPlayer && !canKillPlayer) { return; }

                health.Damage(999999f);
            }
        }
    }

    public override void Interact() {
        if(!_active) {
            Activate();
        } else if(toggleable) {
            Deactivate();
        }
    }

    public override void Activate() {
        _active = true;
    }

    public override void Deactivate() {
        _active = false;
    }

    private void OnDrawGizmos() {
        if(!TryGetComponent<Collider>(out Collider collider)) { return; }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, collider.bounds.size);
        Gizmos.color = Color.white;
    }
}
