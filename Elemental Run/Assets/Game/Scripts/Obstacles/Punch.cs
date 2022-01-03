using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code for punching gloves
public class Punch : MonoBehaviour
{
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession == null)
            gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //if player touches punching gloves, kill that fucker   
            StartCoroutine(gameSession.Kill(false, 4)); //4 for boxing glove
            GetComponent<Collider>().enabled = false;
        }
    }
}
