using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public GameObject p1Wins;
    public GameObject p2Wins;

    public int P1Life;
    public int P2Life;

    public GameObject[] p1Sticks;
    public GameObject[] p2Sticks;


    void Start()
    {
        
    }

 
    void Update()
    {
        if (P1Life <= 0)
        {
            player1.SetActive(false);
            p2Wins.SetActive(true);
        }

        if (P2Life <= 0)
        {
            player2.SetActive(false);
            p1Wins.SetActive(true);
        }
    }


    public void HurtP1()
    {
        P1Life -= 1;

        for(int i = 0; i < p1Sticks.Length; i++)
        {
            if(P1Life > i)
            {
                p1Sticks[i].SetActive(true);
            } else {
                p1Sticks[i].SetActive(false);
            }
        }
    }

    public void HurtP2()
    {
        P2Life -= 1;

        for (int i = 0; i < p2Sticks.Length; i++)
        {
            if (P2Life > i)
            {
                p2Sticks[i].SetActive(true);
            }
            else
            {
                p2Sticks[i].SetActive(false);
            }
        }
    }
}
