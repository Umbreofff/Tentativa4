using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private int a_isWalking;
    private int a_isRunning;

    private Vector2 playerMovementInput;
    private Vector3 playerMovement;
    private Vector3 playerRunMovement;
    private bool isMoving;
    private bool isRunning;
    private float rotationVelocity = 10f;

    [SerializeField] private float velocity;
    [SerializeField] private float runVelocity;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        GetAnimationParameters();

        playerInput.Movement.Walk.started += OnMovementInput;
        playerInput.Movement.Walk.canceled += OnMovementInput;
        playerInput.Movement.Walk.performed += OnMovementInput;

        playerInput.Movement.Run.started += OnRunningInput;
        playerInput.Movement.Run.canceled += OnRunningInput;
    }

    void GetAnimationParameters()
    {
        a_isWalking = Animator.StringToHash("isWalking");
        a_isRunning = Animator.StringToHash("isRunning");
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        playerMovementInput = context.ReadValue<Vector2>();
        playerMovement.x = playerMovementInput.x;
        playerMovement.y = 0.0f;
        playerMovement.z = playerMovementInput.y;

        playerRunMovement.x = playerMovementInput.x * runVelocity;
        playerRunMovement.z = playerMovementInput.y * runVelocity;
        isMoving = playerMovementInput.y != 0 || playerMovementInput.x != 0;
    }

    void OnRunningInput(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AnimationHandler();
        PlayerRotationHandler();    
    }

    private void PlayerRotationHandler()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = playerMovement.x;
        positionToLookAt.y = playerMovement.y;
        positionToLookAt.z = playerMovement.z;
        Quaternion currentRotation = transform.rotation;

        if (isMoving)
        {
            Quaternion lookAtRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = (Quaternion.Slerp(currentRotation, lookAtRotation, rotationVelocity * Time.deltaTime));
        }  
    }

    private void AnimationHandler()
    {
        bool isWalkingA = animator.GetBool(a_isWalking);
        bool isRunningA = animator.GetBool(a_isRunning);

        if (isMoving && !isWalkingA)
        {
            animator.SetBool(a_isWalking, true);
        }
        else if(!isMoving && isWalkingA)
        {
            animator.SetBool(a_isWalking, false);
        }

        if (isMoving && isRunning && !isRunningA)
        {
            animator.SetBool(a_isRunning, true);
        }
        else if (!isMoving || !isRunning && isWalkingA)
        {
            animator.SetBool(a_isRunning, false);
        }

    }

    private void MovePlayer()
    {
        if (isRunning)
        {
            characterController.Move(playerRunMovement * Time.deltaTime * velocity);
        }
        else
        {
            characterController.Move(playerMovement * Time.deltaTime * velocity);
        }
    }

    private void OnEnable()
    {
        playerInput.Movement.Enable();
    }

    private void OnDisable()
    {
        playerInput.Movement.Disable();
    }
}
