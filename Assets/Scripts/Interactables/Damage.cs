using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : InteractableBase {

    // Whether the damage zone can be disabled.
    [SerializeField] private bool toggleable = false;

    // Whether the damage zone can damage the player.
    [SerializeField] private bool damagePlayer = true;

    // Whether the damage zone can damage all other health scripts.
    [SerializeField] private bool damageOther = true;

    // How much damage is applied per second.
    [SerializeField, Min(0f)] private float damagePerSec = 20f;

    private void OnValidate() {
        _interactable = false;
    }

    private void Start() {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider collider) {
        if(_active) {
            if(collider.gameObject.TryGetComponent<Health>(out Health health)) {
                if((health.IsPlayer && !damagePlayer) || (!health.IsPlayer && !damageOther)) { return; }

                health.Damage(damagePerSec * Time.deltaTime);
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

        Gizmos.color = new Vector4(1f, 0f, 0f, 0.1f);
        Gizmos.DrawCube(transform.position, collider.bounds.size);
        Gizmos.color = Color.white;
    }
}
