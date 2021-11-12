using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update

    GameSession gameSession;
    PlayerController player;
    PickupSystem pickupSystem;
    
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
        pickupSystem = FindObjectOfType<PickupSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameSession.lastCheckPointPos = player.transform.position;
            float[] elements = pickupSystem.GetElements();
            for(int i = 0; i < 3; i++)
            {
                gameSession.lastElementsCapacity[i] = elements[i];
            }
           
        }
    }
}
