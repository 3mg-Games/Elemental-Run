using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            
            StartCoroutine(gameSession.Kill(false, 4)); //3for obstacle

        }
    }
}
