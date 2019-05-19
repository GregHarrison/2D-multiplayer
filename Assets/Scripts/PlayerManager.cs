using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManager
{
    public Color playerColor;
    public Transform spawnPoint;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int wins;

    private PlayerController playerController;
    private GunController gun;
    private JoystickAiming aimControl;
    private GameObject canvasGameObject;

    public void Setup()
    {
        playerController = instance.GetComponent<PlayerController>();
        gun = instance.GetComponentInChildren<GunController>();
        aimControl = instance.GetComponentInChildren<JoystickAiming>();

        playerController.playerNumber = playerNumber;
        gun.playerNumber = playerNumber;
        aimControl.playerNumber = playerNumber;

        //Set the player color
        //SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
        //renderer.color = playerColor;
    }

    //Used when player should not be able to controller their character
    public void DisableContol()
    {
        playerController.enabled = false;
        gun.enabled = false;
        aimControl.enabled = false;
    }

    //Used when player should be able to controller their character
    public void EnableControl()
    {
        playerController.enabled = true;
        gun.enabled = true;
        aimControl.enabled = true;
    }

    //Used at the start of each round to put all players in default state
    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;
        instance.transform.localScale = spawnPoint.localScale;

        instance.SetActive(false);
        instance.SetActive(true);

    }
}
