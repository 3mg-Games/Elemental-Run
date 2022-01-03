using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for adding a new pickup as more fuel to player's backpack
public class ElementPickup : MonoBehaviour
{
    [SerializeField] int elementId;  // 0 - fire
                                     // 1 - water
                                     // 2 - earth

    [SerializeField] AudioClip pickUpSfx;   
    [SerializeField] [Range(0, 1f)] float pickUpSfxVolume = 1f;
    PickupSystem pickupSystem;
   // AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
       // audioSource = GetComponent<AudioSource>();
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickUpSfx,
                Camera.main.transform.position,
                pickUpSfxVolume);
            pickupSystem.AddNewElementPickup(elementId);
            Destroy(gameObject);
        }
        
    }
}
