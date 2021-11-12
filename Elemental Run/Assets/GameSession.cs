using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [SerializeField] GameObject elemntSelectionPanel;

    PickupSystem pickupSystem;
    PlayerController player;

    int currTerrainElementId;

    bool isPlayerAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateElementSelectionPanel(int elementTerrainId)
    {
        currTerrainElementId = elementTerrainId;
        Time.timeScale = 0; //stop the player
        elemntSelectionPanel.SetActive(true);
    }

    public void DeactivateElementSelectionPanel(int elementSelectedId)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        elemntSelectionPanel.SetActive(false);


        //also check if wrong fuel to kill player

        switch(currTerrainElementId)
        {

            case 0: //fire
                if (elementSelectedId == 2)
                {
                    isPlayerAlive = false;
                    Kill();
                }

                break;

            case 1: //water
                if (elementSelectedId == 0)
                {
                    isPlayerAlive = false;
                    Kill();
                }

                break;

            case 2:   //earth
                if (elementSelectedId == 1)
                {
                    isPlayerAlive = false;
                    Kill();
                }

                break;
                   
        }

        //code for decreasing fuel level
        if(isPlayerAlive)
        pickupSystem.ConsumeFuel(elementSelectedId);

        Time.timeScale = 1;    //enable player

        
       
    }

    private void Kill()
    {
        player.KillPlayer();
    }
}
