using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Projectile Shoot Forces")]
    [SerializeField] Vector2 shootEmptyBallForce = new Vector2(800, 0);
    [SerializeField] Vector2 shootCapturedPlayerForce= new Vector2(1000, 0);

    [Header("Projectile Logic")]
    [SerializeField] GameObject projectile;
    [SerializeField] LayerMask whatTotHit;
    [SerializeField] KeyCode shoot;
    [SerializeField] float shootTimeDelay = 1f;

    [Header(("Suck Ball"))]
    [SerializeField] float suckDistance = 5f;
    [SerializeField] float suckSpeed = 5f;


    private Transform firePoint;
    private RaycastHit2D suctionTargetHit;
    private CaptureBall capturedPlayer;
    private bool capturedPlayerCarried = false;
    private Vector2 firePointPosition;
    private Vector2 mousePosition;
    private bool canShoot = false;


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
        mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        RotateToMouse();
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
        while (true) // allows to run until game ends or object destroyed
        {
            canShoot = true;
            yield return new WaitForSeconds(shootTimeDelay);
        }
    }


    private void RotateToMouse()
    {
        /*create a direction between the gun origin (which should equal parent origin) and the mouse position. Noramalize this value.
        Multiply by player locale scale so that if the player sprite flips the aim direction is inverted 
        (affectively changing the rotation angle by 180deg and making it appear that gun rotates indepependently of player sprite*/
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position) * transform.parent.localScale.x;
        direction.Normalize();

        // Define angle between the player and mouse using the direction just created. 
        float angleBetweenPlayerAndMouse = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // rotate the gun about its origin based on the angle (which is calculated every frame when RotateTOMouse() is called in Update())
        transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenPlayerAndMouse);
    }


    private void ShootEmptyBall()
    {
        // if player is not carrying and captured player they can press the shoot key to shoot an empty capture ball
        if (canShoot && !capturedPlayerCarried && Input.GetKeyDown(shoot))
        {
            // Instantiate projectile
            GameObject ballClone = Instantiate(projectile, firePointPosition, firePoint.rotation) as GameObject;

            // Add a force to the projectile
            Rigidbody2D ballCloneRigidBody = ballClone.GetComponent<Rigidbody2D>();
            ballCloneRigidBody.AddRelativeForce(shootEmptyBallForce * transform.parent.localScale.x * Time.fixedDeltaTime, ForceMode2D.Impulse);

            canShoot = false;
        }
    }


    private void CreateRaycast()
    {
        // Create a raycast and define what is hit by the raycast as suctionTargetHit
        suctionTargetHit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, suckDistance, whatTotHit);

        // Draw line of raycast FOR TESTING PURPOSES
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.green);
        if (suctionTargetHit.collider != null)
        {
            Debug.DrawLine(firePointPosition, suctionTargetHit.point, Color.red);
        }
    }


    public void SuckBall()
    {
        // while Fire1 positive button held down suck the ball towards player
        if (Input.GetButton("Fire1") && suctionTargetHit)
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
        if (canShoot && capturedPlayerCarried && Input.GetKeyDown(shoot))
        {
            Rigidbody2D capturedPlayerRigidBody = capturedPlayer.GetComponent<Rigidbody2D>();
            capturedPlayerRigidBody.AddRelativeForce(shootCapturedPlayerForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            capturedPlayerCarried = false;
            canShoot = false;
        }
    }
}
