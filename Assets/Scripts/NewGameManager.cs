using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameManager : MonoBehaviour
{

    public float startDelay = 3f;
    public float endDelay = 3f;
    public Text messageText;
    public GameObject playerPrefab;
    public PlayerManager[] players;
    public int numRoundsToWin = 5;

    private int roundNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private PlayerManager roundWinner;
    private PlayerManager gameWinner;

    void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
        SpawnAllPlayers();
        StartCoroutine(GameLoop());
    }

    private void SpawnAllPlayers()
    {
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            players[playerIndex].instance = Instantiate(playerPrefab,
                                                   players[playerIndex].spawnPoint.position,
                                                   players[playerIndex].spawnPoint.rotation) as GameObject;
            players[playerIndex].playerNumber = playerIndex + 1;
            players[playerIndex].Setup();
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        //If there is a game winner restart the scene (TOO CHANGE WHEN MENUS ARE MADE)
        if (gameWinner != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            //If no winner, restart this coroutine to continue game loop. Since this coroutine does not yield, the current version of the game loop will end
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllPlayers();
        DisablePlayerControl();
        roundNumber++;
        messageText.text = "Round " + roundNumber;

        // Wait and then yield return back to game loop
        yield return startWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnablePlayerControl();
        messageText.text = string.Empty;
        while (!OnePlayerLeft())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {
        DisablePlayerControl();

        //Clear previous round winner
        roundWinner = null;

        //Check for a round winner
        roundWinner = GetRoundWinner();

        //If there is a round winner, increment their score
        if (roundWinner != null)
        {
            roundWinner.wins++;
        }

        //Check for game winner
        gameWinner = GetGameWinner();

        //Update screen text
        string message = EndMessage();
        messageText.text = message;

        //Wait and then yield control back to game loop
        yield return endWait;
    }

    private bool OnePlayerLeft()
    {
        int numPlayersLeft = 0;

        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            if (players[playerIndex].instance.activeSelf)
            {
                numPlayersLeft++;
            }
        }

        return numPlayersLeft <= 1;
    }

    private PlayerManager GetRoundWinner()
    {
        //loop through all of the players
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            //If one of the players in active return that player
            if (players[playerIndex].instance.activeSelf)
            {
                return players[playerIndex];
            }
        }

        //If no player are active return null. This is a draw (very rare if possible).
        return null;
    }

    private PlayerManager GetGameWinner()
    {
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            if (players[playerIndex].wins == numRoundsToWin)
            {
                return players[playerIndex];
            }
        }

        return null;
    }

    private string EndMessage()
    {
        // default end message is "DRAW"
        string message = "DRAW";

        //Change message to reflect if there is a round winner
        if (roundWinner != null)
        {
            message = roundWinner + " WINS ROUND " + roundNumber + "!";

            //Add line breaks after initial message
            message += "\n\n\n\n";

            //Add all players scores to the message
            for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
            {
                message += "PLAYER " + players[playerIndex].playerNumber + ": " + players[playerIndex].wins + " ROUNDS WON\n";
            }
        }

        //If there is a game winner anonce the winner
        if (gameWinner != null)
        {
            message = gameWinner + " WINS THE GAME!";
        }

        return message;
    }

    private void ResetAllPlayers()
    {
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            players[playerIndex].Reset();
        }
    }

    private void EnablePlayerControl()
    {
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            players[playerIndex].EnableControl();
        }
    }

    private void DisablePlayerControl()
    {
        for (int playerIndex = 0; playerIndex < players.Length; playerIndex++)
        {
            players[playerIndex].DisableContol();
        }
    }
}
