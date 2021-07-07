using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : InteractableBase {

    [SerializeField] private bool startActive = true;

    // Whether the laser only damages the player.
    [SerializeField] private bool damagePlayerOnly = true;

    [SerializeField] private float damagePerSec = 50f;

    [SerializeField] private int maxLaserAdjustAmt = 10;

    private LineRenderer lineRenderer;

    private Vector3 dir;

    private int index;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        SetupLaser();
        lineRenderer.numCapVertices = lineRenderer.numCornerVertices = 5;

        _active = startActive;
    }

    private void Update() {
        if(_active) {
            SetupLaser();
            EmitLaser();
        } else {
            SetupLaser();
        }
    }

    private void SetupLaser() {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = lineRenderer.endWidth = 0f;

        index = 0;
        dir = transform.forward;
    }

    private void EmitLaser() {

        bool finished = false;

        while(!finished && index < maxLaserAdjustAmt) {
            if(Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), dir, out RaycastHit hitInfo)) {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);

                GameObject obj = hitInfo.collider.gameObject;

                if(obj.TryGetComponent<LaserReceiver>(out LaserReceiver receiver)) {
                    receiver.Activate();

                    finished = true;
                } else if(obj.TryGetComponent<LaserAdjust>(out LaserAdjust adjuster)) {
                    if(adjuster.LaserAdjustType == AdjustType.Reflect) {
                        dir = Vector3.Reflect(dir, hitInfo.normal);
                    } else if(adjuster.LaserAdjustType == AdjustType.Refract) {
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, obj.transform.position + obj.transform.forward * obj.GetComponent<Collider>().bounds.extents.z);

                        dir = obj.transform.forward;
                    }
                } else if(obj.TryGetComponent<Health>(out Health health)) {
                    if(!health.IsPlayer && damagePlayerOnly) { return; }

                    health.Damage(damagePerSec * Time.deltaTime);

                    finished = true;
                } else {
                    finished = true;
                }
            } else {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.GetPosition(lineRenderer.positionCount - 2) + dir * 1000f);

                finished = true;
            }

            index++;
        }

        if(lineRenderer.positionCount > 1) {
            lineRenderer.startWidth = lineRenderer.endWidth = 0.04f;
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
