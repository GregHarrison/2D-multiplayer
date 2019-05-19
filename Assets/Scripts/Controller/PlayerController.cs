using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 16f;
    public int playerNumber = 1;  


    private string xMoveInputAxis;
    private string jumpButton;
    private bool isCaptured = false;
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private CapsuleCollider2D myBodyCollider2D;
    private BoxCollider2D myFeetCollider2D;
    private float gunRotationAngle;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        //Set inputs based on player number
        xMoveInputAxis = "LeftStickX" + playerNumber;
        jumpButton = "B" + playerNumber;
    }

    private void Update()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis(xMoveInputAxis);
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        bool playerGrounded = myFeetCollider2D.IsTouchingLayers(LayerMask.GetMask("Foreground"));

        if (!playerGrounded)
        {
            myAnimator.SetBool("Grounded", false);
            return;
        }
        else
        {
            myAnimator.SetBool("Grounded", true);
        }

        if(Input.GetButtonDown(jumpButton))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }
}
