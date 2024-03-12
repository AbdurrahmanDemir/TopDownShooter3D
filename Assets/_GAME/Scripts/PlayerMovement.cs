using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls playerControls;
    CharacterController characterController;
    public Vector3 movementDirection;

    public float walkSpeed;
    float verticalVelocity;
    float gravityScale=9.81f;
    private Vector2 moveInput;
    private Vector2 aimInput;


    [Header("Aim Info")]
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;

    private void Awake()
    {
        playerControls= new PlayerControls();

        playerControls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        playerControls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        playerControls.Character.Aim.performed+= context => aimInput = context.ReadValue<Vector2>();
        playerControls.Character.Aim.canceled+= context => aimInput = Vector2.zero;
    }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        ApplyMovement();

        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lookingDirection= hitInfo.point-transform.position;
        }
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * walkSpeed);            
        }
    }

    private void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravityScale * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -.5f;

    }

    private void OnEnable()
    {
        playerControls.Enable();

    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
}
