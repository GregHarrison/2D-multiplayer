using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickAiming : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        Vector2 direction = new Vector2(Input.GetAxis("RightStickX"), Input.GetAxis("RightStickY"));
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
