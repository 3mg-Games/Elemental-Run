using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//death by water boundary
//if player falls and touches it, instant death :(
public class DeathBoundary : MonoBehaviour
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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(gameSession.Kill(true, 5));
        }
    }
}
