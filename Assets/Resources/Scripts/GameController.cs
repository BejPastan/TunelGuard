using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //event StartGame
    public delegate void StartGame();
    public static event StartGame OnStartGame;

    private void Start()
    {
        //subscribe to event
        OnStartGame += FindObjectOfType<WaitingRoom>().StartGame;
        OnStartGame += FindObjectOfType<Timer>().StartGame;
        //start game
        OnStartGame();
    }

    //freez time
    public static void FreezTime()
    {
        Time.timeScale = 0;
    }

    //Unfreez time
    public static void UnfreezTime()
    {
        Time.timeScale = 1;
    }
}
