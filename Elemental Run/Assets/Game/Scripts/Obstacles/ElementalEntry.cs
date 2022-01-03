using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code for when player enters elemental terrain
public class ElementalEntry : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [Tooltip("Fire - 0, Water - 1, Earth - 2, Ice - 3")]
    [SerializeField] int elementId = 0;

    bool hasPlayerEntered = false;
    public GameSession gameSession;
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
        if(gameSession == null)
            gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasPlayerEntered)
        {
            //when player enters elemental terrain, acitvate choice selection canvas
            //pickupSystem.ResetAllTerrainSpray();
            hasPlayerEntered = true;
            gameSession.ActivateElementSelectionPanel(elementId);
            
        }
    }
}
