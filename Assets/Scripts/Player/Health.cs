using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    private float health;

    private Vector3 startingPos;

    private bool _isPlayer;
    [HideInInspector] public bool IsPlayer => _isPlayer;

    private void OnValidate() {
        health = startingHealth;

        _isPlayer = (GetComponent<FirstPersonController>() != null);
    }

    private void Start() {
        health = startingHealth;
        startingPos = transform.position;

        _isPlayer = (GetComponent<FirstPersonController>() != null);
    }

    private void Update() {
        // Respawn() the player if their health is zero.
        if(health <= 0) {
            Respawn();
        }
    }

    private void Respawn() {
        health = startingHealth;

        if(TryGetComponent<CharacterController>(out CharacterController charController)) {
            charController.enabled = false;
            transform.position = startingPos;
            charController.enabled = true;

            Interact playerInteract = FindObjectOfType<Interact>();
            if(playerInteract.holdItem != null) {
                playerInteract.holdItem.Deactivate();
                playerInteract.holdItem = null;
            }
        } else {
            transform.position = startingPos;

            Interact playerInteract = FindObjectOfType<Interact>();
            if(playerInteract.holdItem != null && playerInteract.holdItem.gameObject == gameObject) {
                playerInteract.holdItem.Deactivate();
                playerInteract.holdItem = null;
            }

            if(TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.velocity = Vector3.zero;
            }
        }
    }

    // Use this function in other scripts to damage the player.
    public void Damage(float damage) {
        health -= damage;
    }

    // This is another jank fake UI thing for overlaying red on the screen as the player gets damaged;
    private void OnDrawGizmos() {
        if(_isPlayer) {
            float opacity = Mathf.Pow((startingHealth - Mathf.Max(health, 0f)) / startingHealth, 3f);
            Gizmos.color = new Vector4(1f, 0f, 0f, Mathf.SmoothStep(0f, 1f, opacity));
            Camera cam = Camera.main;
            Gizmos.DrawSphere(cam.transform.position + cam.transform.forward, 0.875f);
            Gizmos.color = Color.white;
        }
    }

}
