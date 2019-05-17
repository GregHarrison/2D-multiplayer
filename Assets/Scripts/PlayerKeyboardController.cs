using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKeyboardController : MonoBehaviour {

    public int playerNumber = 1;  //used to identify which which player belongs to which gamer. This is set by this players manager.
    public float moveSpeed = 10f;
    public float jumpForce = 16f;

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;

    private Rigidbody2D theRB;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public bool isGrounded;

    private Animator anim;

    public GameObject snowBall;
    public Transform throwPoint;

    bool isCaptured = false;


    void Awake()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (isCaptured) { return; }

        Run();
        Jump();
        FlipSprite();
    }

    private void Run()
    {

        if (Input.GetKey(left))
        {
            theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
        }
        else if (Input.GetKey(right))
        {
            theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = new Vector2(0, theRB.velocity.y);
        }
        anim.SetFloat("Speed", Mathf.Abs(theRB.velocity.x));
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        anim.SetBool("Grounded", isGrounded);
        if (Input.GetKeyDown(jump) && isGrounded)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }
    }

    public void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(theRB.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(theRB.velocity.x), 1f);
        }
    }

    public void Captured()
    {
        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("Speed", 0f);
        isCaptured = true;

        // prevents another trigger collider from interacting with player
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
