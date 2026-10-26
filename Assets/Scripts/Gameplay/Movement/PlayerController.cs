using System.Runtime.InteropServices;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour  //This is using "Send messages" as the main player input behaviour
{
    //Variables
    private Rigidbody rb;
    private PlayerInputs inputs;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpHeight;
    private float movementX;
    private float movementY;
    private bool canJump;
    private bool isGrounded;
    [SerializeField] LayerMask layerMask;

    void Awake()
    {
        inputs = new PlayerInputs();
    }

    void OnEnable()
    {
        inputs.Player.Enable();
        inputs.Player.Jump.started += OnJump;
        inputs.Player.Jump.performed += OnJump;
        inputs.Player.Jump.canceled += OnJump;

        inputs.Player.Fire.started += OnFire;
        inputs.Player.Fire.performed += OnFire;
        inputs.Player.Fire.canceled += OnFire;


        inputs.Player.Move.started += OnMove;
        inputs.Player.Move.performed += OnMove;
        inputs.Player.Move.canceled += OnMove;

    }

    void OnDisable()
    {
        inputs.Player.Disable();
        inputs.Player.Jump.started -= OnJump;
        inputs.Player.Jump.performed -= OnJump;
        inputs.Player.Jump.canceled -= OnJump;

        inputs.Player.Move.started -= OnMove;
        inputs.Player.Move.performed -= OnMove;
        inputs.Player.Move.canceled -= OnMove;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY).normalized;
        rb.AddForce(movement * moveSpeed);

        //Raycast 
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, layerMask)) //10f was a bit too much, as it would still detect the ground
        {
            if (hit.collider)
            {
                isGrounded = true;
                Debug.DrawRay(transform.position, Vector3.down, Color.red);
                Debug.Log("Hit: " + hit.collider);
            }
        }
    }

    #region Input methods
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementVector = context.ReadValue<Vector2>();

        if (context.started)
        {
            return;
        }

        if (context.performed)
        {
            movementX = movementVector.x;
            movementY = movementVector.y;

    
        }
        else if (context.canceled)
        {
            movementX = 0f;
            movementY = 0f;
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            return;
        }
        if (context.performed)
        {
            Debug.Log("Fire!");
        }
        // else if (context.canceled)
        // {
        //     Debug.Log("Action Cancelled");
        //  }
              
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            return;
        }

        if (isGrounded && !canJump) //if the player is grounded AND can jump is false, allow the player to jump
        {
            canJump = true;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
        }
        else if (context.canceled)
        {
            //Reset the values as the player will be in the air
            isGrounded = false;
            canJump = false;
        }

    }

    #endregion

}
