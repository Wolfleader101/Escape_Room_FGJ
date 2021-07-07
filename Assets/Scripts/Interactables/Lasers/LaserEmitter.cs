using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(LineRenderer))]
public class LaserEmitter : InteractableBase {

    // Whether the laser should start in an active start.
    [SerializeField] private bool startActive = true;

    // The thickness of the laser. This should be kept as a uniform value across a theme, for consistency.
    [SerializeField] private float laserThickness = 0.04f;

    // Whether the laser only damages the player.
    [SerializeField] private bool damagePlayerOnly = true;

    // How much damage is applied to the player over a second.
    [SerializeField] private float damagePerSec = 50f;

    // The maximum amount of LaserAdjust objects the laser will interact with before terminating.
    [SerializeField] private int maxLaserAdjustAmt = 100;

    private LineRenderer lineRenderer;

    // The current move direction for the laser.
    private Vector3 dir;

    // The index of how many LaserAdjust objects have been detected.
    private int index;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        SetupLaser();
        lineRenderer.numCapVertices = lineRenderer.numCornerVertices = 5;

        _active = startActive;
    }

    private void Update() {
        // If _active, fire the laser, otherwise disable it.
        if(_active) {
            SetupLaser();
            EmitLaser();
        } else {
            SetupLaser();
        }
    }

    // This script acts like both a reset and a disable for the laser. It resets all values back to the start and also sets the width of the laser to 0, effectively disabling it.
    private void SetupLaser() {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = lineRenderer.endWidth = 0f;

        index = 0;
        dir = transform.forward;
    }

    // This scripts fires the laser through the world, interacting with LaserAdjust objects to change the current direction of travel.
    private void EmitLaser() {

        // Boolean variable to end the while loop after the laser has found an end point.
        bool finished = false;

        // This while loop will continue until either the laser has found an end point or the laser has reached its max amount of direction adjustments.
        while(!finished && index < maxLaserAdjustAmt) {
            if(Physics.Raycast(lineRenderer.GetPosition(lineRenderer.positionCount - 1), dir, out RaycastHit hitInfo)) {
                // If we hit something with the raycast, add a new point in the laser path at the point of collision.
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hitInfo.point);

                GameObject obj = hitInfo.collider.gameObject;

                if(obj.TryGetComponent<LaserReceiver>(out LaserReceiver receiver)) {
                    // If the object we hit is a laser receiver, Activate() the receiver and end the laser path loop.
                    receiver.Activate();

                    finished = true;

                } else if(obj.TryGetComponent<LaserAdjust>(out LaserAdjust adjuster)) {
                    // If the object is a LaserAdjust type:
                    if(adjuster.LaserAdjustType == AdjustType.Reflect) {
                        // If it is a Reflect object, set the new direction of the laser to the reflection of the current direction across the collision surface's normal.
                        dir = Vector3.Reflect(dir, hitInfo.normal);
                    } else if(adjuster.LaserAdjustType == AdjustType.Refract) {
                        /* If it is a Refract object, set a new laser path point to the middle of the face of the object which is facing in the direction of
                            the Refract objects forward direction. */
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, obj.transform.position + obj.transform.forward * obj.GetComponent<Collider>().bounds.extents.z);

                        // Set the laser's new direction to the forward direction of the Refract object.
                        dir = obj.transform.forward;

                        /* NOTE: This is not physically accurate for refraction, however is meant to be a different way to control lasers, other than simply reflecting
                            them off a surface. It gives players an easier way to aim a laser. */
                    }

                } else if(obj.TryGetComponent<Health>(out Health health)) {
                    /* If the object has health, we first make sure that we dont damage anything other than the player if we dont want to, then apply the damage and then
                        end the laser path. */
                    if(!health.IsPlayer && damagePlayerOnly) { return; }

                    health.Damage(damagePerSec * Time.deltaTime);

                    finished = true;

                } else {
                    // If we do not hit an object with a script we care about, it is most likely a static level object. Simply end the path of the laser.
                    finished = true;
                }
            } else {
                // If the raycast does not hit a point, then the end point is simply set to a point 1000 units away from the last laser point, in the current direction of its travel.
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, lineRenderer.GetPosition(lineRenderer.positionCount - 2) + dir * 1000f);

                finished = true;
            }

            // Increment the counter for the amount of LaserAdjust objects that have been interacted with.
            index++;
        }

        // Set the laser to the desired thickness value.
        lineRenderer.startWidth = lineRenderer.endWidth = laserThickness;
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
