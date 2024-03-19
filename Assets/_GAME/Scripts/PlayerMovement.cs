using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Player player;

    private PlayerControls playerControls;
    CharacterController characterController;
    public Vector3 movementDirection;
    private Animator animator;

    bool isRunning;
    float speed;
    public float runSpeed;
    public float walkSpeed;
    float verticalVelocity;
    float gravityScale=9.81f;
    private Vector2 moveInput;
    private Vector2 aimInput;


    [Header("Aim Info")]
    public Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;

    AnimatorClipInfo[] animatorinfo;
    string current_animation;

    private void Start()
    {
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        animator= GetComponentInChildren<Animator>();
        speed = walkSpeed;
        AssingInputEvents();
    }
    private void Update()
    {
        ApplyMovement();
        AimTowardsMouse();
        AnimatorControllers();

        animatorinfo = this.animator.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;
        Debug.Log(current_animation);
    }

    private void Shoot()
    {
        animator.SetTrigger("Fire");
    }

    private void AnimatorControllers()
    {
        float xVelocity = Vector3.Dot(movementDirection.normalized, transform.right);
        float zVelocity = Vector3.Dot(movementDirection.normalized, transform.forward);

        animator.SetFloat("xVelocity", xVelocity, .1f, Time.deltaTime);
        animator.SetFloat("zVelocity", zVelocity, .1f, Time.deltaTime);

        bool playRunAnimation = isRunning && movementDirection.magnitude > 0;
        animator.SetBool("isRunning", playRunAnimation);
    }

    private void AimTowardsMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lookingDirection = hitInfo.point - transform.position;
            lookingDirection.y = 0f;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;

            aim.position = new Vector3(hitInfo.point.x, transform.position.y+1, hitInfo.point.z);
        }
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * speed);            
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
    #region new ýnput system
    private void AssingInputEvents()
    {
        playerControls = player.controls;

        playerControls.Character.Fire.performed += context => Shoot();

        playerControls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        playerControls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        playerControls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        playerControls.Character.Aim.canceled += context => aimInput = Vector2.zero;


        playerControls.Character.Run.performed += context =>
        {
            speed = runSpeed;
            isRunning = true;
        };
        playerControls.Character.Run.canceled += context =>
        {
            speed = walkSpeed;
            isRunning = false;
        };
    }
    #endregion
}
