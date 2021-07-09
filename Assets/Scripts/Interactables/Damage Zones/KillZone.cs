using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : InteractableBase {

    // Whether the zone should start as active.
    [SerializeField] private bool startActive = true;

    // Whether the damage zone can be disabled.
    [SerializeField] private bool toggleable = false;

    /* Whether the kill zone can damage the player. This is useful for creating zones at the end of levels to destroy all objects that arent the player.
        This prevents the player from bringing items to other levels. */
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

    private void OnDrawGizmosSelected() {
        if(!TryGetComponent<Collider>(out Collider collider)) { return; }

        Gizmos.color = new Vector4(1f, 0f, 0f, 0.25f);
        Gizmos.DrawCube(transform.position, collider.bounds.size);
        Gizmos.color = Color.white;
    }
}
