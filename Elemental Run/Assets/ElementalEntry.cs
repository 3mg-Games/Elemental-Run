using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalEntry : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [Tooltip("Fire - 0, Water - 1, Earth - 2")]
    [SerializeField] int elementId = 0;

    GameSession gameSession;
    //PickupSystem pickupSystem;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
       // pickupSystem = FindObjectOfType<PickupSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //pickupSystem.ResetAllTerrainSpray();
            gameSession.ActivateElementSelectionPanel(elementId);
        }
    }
}
