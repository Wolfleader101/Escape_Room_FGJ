using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    [SerializeField] private float startingHealth = 100f;

    private float health;

    private Vector3 startingPos;

    private bool _isPlayer;
    [HideInInspector] public bool IsPlayer => _isPlayer;

    private void Start() {
        health = startingHealth;
        startingPos = transform.position;

        _isPlayer = (GetComponent<FirstPersonController>() != null);
    }

    private void Update() {
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
        } else {
            transform.position = startingPos;
        }


    }

    public void Damage(float damage) {
        health -= damage;
    }

}
