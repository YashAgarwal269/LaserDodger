using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody myBody;
    private Animator anim;
    private bool isPlayerMoving;

    private float playerSpeed = 15f;
    private float rotationSpeed = 100f;

    private float jumpForce = 500f; // Adjusted jump force value
    private bool canJump;

    private float moveHorizontal, moveVertical;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private Vector2 touchStartPos;
    public float swipeSensitivity = 0.1f;

    public SimpleJoystick movementJoystick; // Ensure this is the correct joystick class

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock the cursor to the center of the screen
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerMoveKeyboard();
        AnimatePlayer();
        IsOnGround();
    }

    private void FixedUpdate()
    {
        MoveAndRotate();
    }

    private void PlayerMoveKeyboard()
    {
        if (movementJoystick != null)
        {
            moveHorizontal = movementJoystick.Horizontal;
            moveVertical = movementJoystick.Vertical;

            // Debugging output
            Debug.Log($"Horizontal: {moveHorizontal}, Vertical: {moveVertical}");
        }
        else
        {
            Debug.LogError("Movement Joystick is not assigned.");
        }
    }

    private void MoveAndRotate()
    {
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = direction * playerSpeed * Time.deltaTime;
            myBody.MovePosition(transform.position + moveDir);
        }

        HandleRotationBySwipe();
    }

    private void AnimatePlayer()
    {
        if (moveVertical != 0 || moveHorizontal != 0)
        {
            if (!isPlayerMoving)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    isPlayerMoving = true;
                    anim.SetTrigger("Run");
                }
            }
        }
        else
        {
            if (isPlayerMoving)
            {
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    isPlayerMoving = false;
                    anim.SetTrigger("Stop");
                }
            }
        }
    }

    private void IsOnGround()
    {
        canJump = Physics.Raycast(groundCheck.position, Vector3.down, 0.2f, groundLayer);
    }

    public void JumpButtonPressed()
    {
        if (canJump)
        {
            canJump = false;
            myBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    private void HandleRotationBySwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is on the right half of the screen
            if (touch.position.x > Screen.width / 2)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 swipeDelta = touch.position - touchStartPos;

                    // Calculate rotation based on swipe sensitivity
                    float rotationAmount = swipeDelta.x * swipeSensitivity;

                    // Rotate player around Y axis based on swipe direction
                    transform.Rotate(Vector3.up, rotationAmount);

                    // Update touch start position for the next frame
                    touchStartPos = touch.position;
                }
            }
        }
    }
}
