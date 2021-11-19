﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalWall : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [Tooltip("Fire - 0, Water - 1, Earth - 2")]
    [SerializeField] int elementId = 0;
    [Tooltip("Fire - 0, Water - 1, Earth - 2")]
    [SerializeField] int counterElementNeededId = 0;

    PickupSystem pickupSystem;
    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pickupSystem.ElementalWallCheck(elementId, counterElementNeededId, gameObject);

        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
