﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPickup : MonoBehaviour
{
    [SerializeField] int elementId;  // 0 - fire
                                     // 1 - water
                                     // 2 - earth

   
    PickupSystem pickupSystem;
    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            pickupSystem.AddNewElementPickup(elementId);
            Destroy(gameObject);
        }
        
    }
}
