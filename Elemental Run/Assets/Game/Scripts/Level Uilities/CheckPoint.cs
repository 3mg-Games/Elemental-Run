﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Start is called before the first frame update

    GameSession gameSession;
    PlayerController player;
    PickupSystem pickupSystem;
    Animator animator;
    
    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
        pickupSystem = FindObjectOfType<PickupSystem>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameSession == null)
            gameSession = FindObjectOfType<GameSession>();
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

            gameSession.playerDir = player.GetDir();

            gameSession.isClampX = player.GetIsClampX();
            gameSession.isClampZ = player.GetIsClampZ();

            var clampLimits = player.GetClampLimits();
            gameSession.clampLowerLimit = clampLimits.x;
            gameSession.clampUpperLimit = clampLimits.y;

            gameSession.SetTwoChoiceCount();

            animator.SetTrigger("Activate");
        }
    }
}
