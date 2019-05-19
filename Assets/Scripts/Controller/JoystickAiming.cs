using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickAiming : MonoBehaviour
{
    public int playerNumber = 1;
    private string xAimInputAxis;
    private string yAimInputAxis;

    private void Start()
    {
        //Set aim input axis based on player number
        xAimInputAxis = "RightStickX" + playerNumber;
        yAimInputAxis = "RightStickY" + playerNumber;
    }

    void Update()
    {
        Aim();
        FlipSprites(Aim());
    }

    public Vector2 Aim()
    {
        Vector2 gunDirection = new Vector2(Input.GetAxis(xAimInputAxis), Input.GetAxis(yAimInputAxis));
        gunDirection.Normalize();
        float angle = Mathf.Atan2(gunDirection.y, gunDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        return gunDirection;
    }

    private void FlipSprites(Vector2 gunDirection)
    {
        if (gunDirection.x < 0)
        {
            transform.parent.localScale = new Vector2(-1, 1);
            transform.localScale = new Vector2(-1, -1);
        }
        else
        {
            transform.parent.localScale = Vector2.one;
            transform.localScale = Vector2.one;
        }
    }
}
