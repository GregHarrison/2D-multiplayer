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
    [SerializeField] LayerMask whatTotHit;
    [SerializeField] float shootTimeDelay = 1f;

    [Header(("Suck Ball"))]
    [SerializeField] float suckDistance = 5f;
    [SerializeField] float suckSpeed = 5f;

    private Transform firePoint;
    private bool canShoot = false;
    private bool capturedPlayerCarried = false;
    private RaycastHit2D suctionTargetHit;

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
        StartCoroutine(ShootTimer());
    }

    void Update()
    {
        CreateRaycast();
    }

    private void FixedUpdate()
    {
        ShootEmptyBall();
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
        float rightTriggerInput = Input.GetAxis("R2");

        // if player is not carrying and captured player they can press the shoot key to shoot an empty capture ball
        if (canShoot && !capturedPlayerCarried && rightTriggerInput != 0) //THIS IS BROKEN = SHOOTS BALL RANDOMLY!!!
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
        suctionTargetHit = Physics2D.Raycast(firePoint.position, firePoint.right, suckDistance, whatTotHit);

        // Draw line of raycast FOR TESTING PURPOSES
        Debug.DrawLine(firePoint.position, firePoint.right * 100, Color.green);
        if (suctionTargetHit.collider != null)
        {
            Debug.DrawLine(firePoint.position, suctionTargetHit.point, Color.red);
        }
    }
}
