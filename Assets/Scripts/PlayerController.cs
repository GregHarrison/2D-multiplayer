using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 16f;
    public int playerNumber = 1;  //used to identify which which player belongs to which gamer. This is set by this players manager.
    private bool isCaptured = false;

    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeetCollider2D;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollider2D = GetComponent<BoxCollider2D>();
        //cache other referenced components
    }

    private void Update()
    {
        Run();
        Jump();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("LeftJoystickHorizontal");
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

        if(CrossPlatformInputManager.GetButtonDown("B Button"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }
}
