using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureBall : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D capturedPlayer)
    {
        if (capturedPlayer.tag == "Player2")
        {
            // On collision add a fixed joint connecting the capture ball to the player at the player transform's position
            var joint = gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = capturedPlayer.attachedRigidbody;
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = new Vector2(0, 0);
            capturedPlayer.attachedRigidbody.freezeRotation = false;

            // Make captured player a child of the ball. This prevents lag between ball and player.
            capturedPlayer.transform.parent = transform;

            // call is captured script in PlayerController
            FindObjectOfType<PlayerKeyboardController>().Captured(); //CHANGE IF WANTING TO REFERENCE PLAYERKEYBOARDCONTROLLER
        }
    }
}
