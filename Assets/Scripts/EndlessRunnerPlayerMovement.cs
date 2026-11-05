using UnityEngine;
using UnityEngine.InputSystem;

public class EndlessRunnerPlayerMovement : MonoBehaviour
{
    [Header("Lane Movement")]
    public float laneDistance = 2f;
    public float moveSpeed = 8f;

    public int currentLane = 1;

    private float targetX = 0f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 0.8f;

    private float jumpTimer = 0f;
    private float currentJumpHeight = 0f;

    private bool isJumping = false;
    public int CurrentLane => currentLane;

    private Vector3 startPosition;
    
    private Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        startPosition = transform.position;

        currentLane = 1;
        targetX = 0f;
    }

    private void Update()
    {
        HandleLaneInput();
        HandleJumpInput();

        MovePlayer();
        UpdateJump();
    }

    private void HandleLaneInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            Debug.Log("Went Left");
            currentLane--;

            currentLane = Mathf.Clamp(currentLane, 0, 2);

            targetX = (currentLane - 1) * laneDistance;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            Debug.Log("Went Right");
            currentLane++;

            currentLane = Mathf.Clamp(currentLane, 0, 2);

            targetX = (currentLane - 1) * laneDistance;
        }
    }


    private void MovePlayer()
    {
        Vector3 currentPosition = transform.position;

        float newX = Mathf.Lerp(
            currentPosition.x,
            targetX,
            moveSpeed * Time.deltaTime
        );

        float movementDirection = 0f;

        if (Mathf.Abs(targetX - newX) < 0.01f)
        {
            newX = targetX;
        }
        else
        {
            movementDirection = Mathf.Sign(
                newX - currentPosition.x
            );
        }

        transform.position = new Vector3(
            newX,
            startPosition.y + currentJumpHeight,
            startPosition.z
        );

        if (playerAnimator != null)
        {
            playerAnimator.SetFloat(
                "MoveDirection",
                movementDirection
            );
        }
    }


    private void HandleJumpInput()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && !isJumping)
        {
            isJumping = true;
            jumpTimer = 0f;
        }
    }

    private void UpdateJump()
    {
        if (!isJumping) return;

        jumpTimer += Time.deltaTime;

        float jumpProgress = jumpTimer / jumpDuration;

        currentJumpHeight = Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;

        if (jumpTimer >= jumpDuration)
        {
            currentJumpHeight = 0f;
            jumpTimer = 0f;
            isJumping = false;
        }
    }

    public float GetJumpHeight()
    {
        return currentJumpHeight;
    }
}
