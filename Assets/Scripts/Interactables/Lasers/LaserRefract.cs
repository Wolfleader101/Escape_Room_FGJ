using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserRefract : LaserReceiver {

    private LineRenderer lineRenderer;

    private float width;

    private void OnValidate() {
        _interactable = false;
    }

    private void Start() {
        _interactable = false;

        Vector3 size = GetComponent<Collider>().bounds.size;
        width = Mathf.Max(Mathf.Max(size.x, size.y), size.z);

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
        lineRenderer.numCapVertices = 5;
        lineRenderer.startWidth = lineRenderer.endWidth = 0f;
    }

    private void Update() {
        if(!_active) {
            lineRenderer.startWidth = lineRenderer.endWidth = 0f;
        }
    }

    private void EmitLaser() {
        lineRenderer.SetPosition(0, hitPoint);
        //lineRenderer.SetPosition(1, transform.position + transform.forward * halfWidth);

        if(Physics.Raycast(new Vector3(transform.position.x, hitPoint.y, transform.position.z), transform.forward, out RaycastHit hitInfo)) {
            Physics.Raycast(hitInfo.point, -transform.forward, out RaycastHit backHitInfo);
            lineRenderer.SetPosition(1, backHitInfo.point);

            lineRenderer.SetPosition(2, hitInfo.point);
            lineRenderer.startWidth = lineRenderer.endWidth = Mathf.Lerp(0.035f, 0.045f, Mathf.Sin(Time.timeSinceLevelLoad * 150f));

            GameObject obj = hitInfo.collider.gameObject;

            if(obj.TryGetComponent<LaserReceiver>(out LaserReceiver receiver)) {
                receiver.Activate();
                receiver.hitPoint = hitInfo.point;
                receiver.damagePerSec = damagePerSec;
                receiver.damagePlayerOnly = damagePlayerOnly;
            } else if(obj.TryGetComponent<Health>(out Health health)) {
                if(!health.IsPlayer && damagePlayerOnly) { return; }

                health.Damage(damagePerSec * Time.deltaTime);
            }
        } else {
            Physics.Raycast(transform.position + transform.forward * width, -transform.forward, out RaycastHit backHitInfo);
            Vector3 pos = new Vector3(backHitInfo.point.x, hitPoint.y, backHitInfo.point.z);
            lineRenderer.SetPosition(1, pos);
            lineRenderer.SetPosition(2, pos + transform.forward * 1000f);
            lineRenderer.startWidth = lineRenderer.endWidth = Mathf.Lerp(0.035f, 0.045f, Mathf.Sin(Time.timeSinceLevelLoad * 150f));
        }
    }

    public override void Activate() {
        base.Activate();

        EmitLaser();
    }

    public override void Deactivate() {
        base.Deactivate();
    }
}
