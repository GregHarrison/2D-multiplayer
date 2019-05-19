using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    [Header("Projectile Shoot Forces")]
    [SerializeField] Vector2 shootEmptyBallForce = new Vector2(800, 0);
    [SerializeField] Vector2 shootCapturedPlayerForce = new Vector2(1000, 0);

    [Header("Projectile Logic")]
    [SerializeField] GameObject projectile;
    [SerializeField] LayerMask whatToHit;
    [SerializeField] float shootTimeDelay = 1f;

    [Header(("Suck Ball"))]
    [SerializeField] float suckDistance = 5f;
    [SerializeField] float suckSpeed = 25f;

    public int playerNumber = 1;

    private string shootButton;
    private string suckButton;
    private Transform firePoint;
    private bool canShoot = false;
    private bool capturedPlayerCarried = false;
    private RaycastHit2D suctionTargetHit;
    private CaptureBall capturedPlayer;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firePoint?");
        }
    }

    private void Start()
    {
        //set inputs based on player number
        shootButton = "R2" + playerNumber;
        suckButton = "L2" + playerNumber;

        StartCoroutine(ShootTimer());
    }

    void Update()
    {
        CreateRaycast();
        RotatePositionOfCapturedPlayer();
    }

    private void FixedUpdate()
    {
        ShootEmptyBall();
        SuckBall();
        ShootCapturedPlayer();
    }

    IEnumerator ShootTimer()
    {
        while (true)
        {
            canShoot = true;
            yield return new WaitForSeconds(shootTimeDelay);
        }
    }

    private void ShootEmptyBall()
    {
        // if player is not carrying and captured player they can press the shoot key to shoot an empty capture ball
        if (canShoot && !capturedPlayerCarried && Input.GetButtonDown(shootButton))
        {
            // Instantiate projectile
            GameObject ballClone = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;

            // Add a force to the projectile
            Rigidbody2D ballCloneRigidBody = ballClone.GetComponent<Rigidbody2D>();
            ballCloneRigidBody.AddRelativeForce(shootEmptyBallForce * transform.parent.localScale.x * Time.fixedDeltaTime, ForceMode2D.Impulse);

            canShoot = false;
        }
    }

    private void CreateRaycast()
    {
        // Create a raycast and define what is hit by the raycast as suctionTargetHit
        suctionTargetHit = Physics2D.Raycast(firePoint.position, firePoint.right, suckDistance, whatToHit);

        // Draw line of raycast FOR TESTING PURPOSES
        Debug.DrawLine(firePoint.position, firePoint.right * 100, Color.green);
        if (suctionTargetHit.collider != null)
        {
            Debug.DrawLine(firePoint.position, suctionTargetHit.point, Color.red);
        }
    }


    public void SuckBall()
    {
        // while Fire1 positive button held down suck the ball towards player
        if (Input.GetButton(suckButton) && suctionTargetHit)
        {
            capturedPlayer = suctionTargetHit.transform.GetComponent<CaptureBall>();
            if (capturedPlayer != null)
            {
                capturedPlayer.transform.position = Vector2.MoveTowards(suctionTargetHit.transform.position, firePoint.position, suckSpeed * Time.fixedDeltaTime);
                if (capturedPlayer.transform.position == firePoint.position)
                {
                    capturedPlayerCarried = true;
                }
                else
                {
                    capturedPlayerCarried = false;
                }
            }

        }
    }


    private void RotatePositionOfCapturedPlayer()
    {
        if (capturedPlayerCarried && capturedPlayer != null)
        {
            capturedPlayer.transform.position = firePoint.position;
            if (transform.parent.localScale.x == -1)
            {
                capturedPlayer.transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, 180);
            }
            else
            {
                capturedPlayer.transform.rotation = transform.rotation;
            }
        }
    }


    private void ShootCapturedPlayer()
    {
        if (canShoot && capturedPlayerCarried && Input.GetButtonDown(shootButton))
        {
            Rigidbody2D capturedPlayerRigidBody = capturedPlayer.GetComponent<Rigidbody2D>();
            capturedPlayerRigidBody.AddRelativeForce(shootCapturedPlayerForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            capturedPlayerCarried = false;
            canShoot = false;
        }
    }
}
