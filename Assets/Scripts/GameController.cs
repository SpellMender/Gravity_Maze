using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public GameObject victoryMsg;
    public GameObject instructions;

    //These boolean variables determine direction of gravity
    public bool gravX = false; //Does gravity work in the x direction
    public bool gravPositive = false; //Is gravity positive

    public float cameraSmoothing; //a modifier for camera rotation rate relative to the player

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Victory()
    {
        victoryMsg.SetActive(true);
        instructions.SetActive(false);
        instance.GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Victory();
    }
}
