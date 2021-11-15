using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalExit : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
   


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
            pickupSystem.ResetAllTerrainSpray();
            
        }
    }
}
