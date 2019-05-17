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

    private PlayerKeyboardController playerController;
    private GameObject canvasGameObject;

    public void Setup()
    {
        playerController = instance.GetComponent<PlayerKeyboardController>();
        canvasGameObject = instance.GetComponentInChildren<Canvas>().gameObject;

        playerController.playerNumber = playerNumber;
    }

    public void EnableControl()
    {
        playerController.enabled = true;
        canvasGameObject.SetActive(true);
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;
        instance.transform.localScale = spawnPoint.localScale;

        instance.SetActive(false);
        instance.SetActive(true);

    }
}
