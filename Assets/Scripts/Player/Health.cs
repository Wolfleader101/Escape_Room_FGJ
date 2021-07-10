using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    [SerializeField] private bool allowHealthRegen = true;

    [SerializeField] private float regenRatePerSec = 50f;

    // This element controls the fade to red for the player. You can leave this field empty if not being attached to the player.
    [SerializeField] private Image healthFadeUI;

    private float health;

    private Vector3 startingPos;

    private bool hasBeenDamaged = false;

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

    private void FixedUpdate() {
        // Respawn() the player if their health is zero.
        if(health <= 0) {
            Respawn();
            return;
        }

        // If object is not currently being damaged, regenerate the health back to full.
        if(!hasBeenDamaged && allowHealthRegen) {
            health = Mathf.Min(health + regenRatePerSec * Time.deltaTime, startingHealth);
        }

        // Set the screen fade UI element
        if(_isPlayer && healthFadeUI != null) {
            float opacity = Mathf.Pow((startingHealth - Mathf.Max(health, 0f)) / startingHealth, 2f);
            healthFadeUI.color = new Vector4(1f, 0f, 0f, Mathf.SmoothStep(0f, 1f, opacity));
        }

        hasBeenDamaged = false;
    }

    private void Respawn() {
        health = startingHealth;

        if(_isPlayer) {
            // To teleport an object with a CharacterController, you need to disable the controller first before teleporting it.
            CharacterController charController = GetComponent<CharacterController>();

            charController.enabled = false;
            transform.position = startingPos;
            charController.enabled = true;

            // Deactivate() held item and unlink it from the player.
            Interact playerInteract = FindObjectOfType<Interact>();
            if(playerInteract.holdItem != null) {
                playerInteract.holdItem.Deactivate();
                playerInteract.holdItem = null;
            }
        } else {
            transform.position = startingPos;

            // Deactivate() held item and unlink it from the player if this object is the current held object.
            Interact playerInteract = FindObjectOfType<Interact>();
            if(playerInteract.holdItem != null && playerInteract.holdItem.gameObject == gameObject) {
                playerInteract.holdItem.Deactivate();
                playerInteract.holdItem = null;
            }

            // Set velocity to zero so it doesnt inherit any velocity from the point of death.
            if(TryGetComponent<Rigidbody>(out Rigidbody rb)) {
                rb.velocity = Vector3.zero;
            }
        }
    }

    // Use this function in other scripts to damage the player.
    public void Damage(float damage) {
        health -= damage;
        hasBeenDamaged = true;
    }
}
