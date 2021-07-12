using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour {
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private float mouseSens = 90f;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private CharacterController characterController;

    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float jumpHeight = 2f;

    [SerializeField] private float gravityScale = -9.81f;

    private float _mouseX;
    private float _mouseY;
    private float _xRotation = 0f;

    private float _xPos;
    private float _zPos;


    private Vector3 _velocity;

    private void Start() {
        if(playerInput == null) playerInput = GetComponent<PlayerInput>();
        if(characterController == null) characterController = GetComponent<CharacterController>();

        //lock cursor to mid
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        if(characterController.isGrounded && _velocity.y < 0) {
            _velocity.y = -2f;
        }
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * _mouseX);

        Vector3 motion = transform.right * _xPos + transform.forward * _zPos;
        characterController.Move(motion * moveSpeed * Time.deltaTime);

        _velocity.y += gravityScale * Time.deltaTime;
        characterController.Move(_velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext value) {
        Vector2 direction = value.ReadValue<Vector2>();
        _xPos = direction.x;
        _zPos = direction.y;
    }

    public void OnLook(InputAction.CallbackContext value) {
        Vector2 direction = value.ReadValue<Vector2>();
        _mouseX = direction.x * mouseSens * Time.deltaTime;
        _mouseY = direction.y * mouseSens * Time.deltaTime;
    }

    public void OnJump(InputAction.CallbackContext value) {
        if(value.started && characterController.isGrounded) {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale);
        }
    }

    public void OnLevelSelect(InputAction.CallbackContext value) {
        SceneManager.LoadScene(0);
    }

    public void OnQuit(InputAction.CallbackContext value) {
        Application.Quit();
    }
}