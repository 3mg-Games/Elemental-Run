﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clamp : MonoBehaviour
{
    //This script clamps the horizontal swerve movement depending upon the direction player is facing
    [SerializeField] float clampLowerLimit = -1.5f;
    [SerializeField] float clampUpperLmit = 1.5f;
    [SerializeField] bool isClampX = false;
    [SerializeField] bool isClampZ = true;

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Clamp happened");
            player.SetClamp(clampLowerLimit, clampUpperLmit, isClampX, isClampZ);
        }
    }
}
