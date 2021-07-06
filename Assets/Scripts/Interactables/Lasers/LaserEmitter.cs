using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : InteractableBase {

    [SerializeField] private bool startActive = true;

    // Whether the laser only damages the player.
    [SerializeField] private bool damagePlayerOnly = true;

    [SerializeField] private float damagePerSec = 20f;

    private LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
        lineRenderer.SetPosition(0, transform.position);

        _active = startActive;
    }

    private void Update() {
        if(_active) {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo)) {
                lineRenderer.SetPosition(1, hitInfo.point);
                lineRenderer.startWidth = lineRenderer.endWidth = Mathf.Lerp(0.035f, 0.045f, Mathf.Sin(Time.timeSinceLevelLoad * 150f));

                GameObject obj = hitInfo.collider.gameObject;

                if(obj.TryGetComponent<ReceiveLaserBase>(out ReceiveLaserBase receiver)) {
                    receiver.Activate();
                    receiver.hitPoint = hitInfo.point;
                } else if(obj.TryGetComponent<Health>(out Health health)) {
                    if(!health.IsPlayer && damagePlayerOnly) { return; }

                    health.Damage(damagePerSec * Time.deltaTime);
                }

            } else {
                lineRenderer.SetPosition(1, transform.position + transform.forward * 1000f);
            }
        } else {
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.startWidth = lineRenderer.endWidth = 0f;
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
