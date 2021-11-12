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
    [SerializeField] float choiceWaitTime = 10f;

    PickupSystem pickupSystem;
    PlayerController player;

    int currTerrainElementId;

    bool isPlayerAlive = true;
    bool isChoiceWaitTimerActive = false;

    float choiceWaitTimer;

    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        choiceWaitTimer = choiceWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(isChoiceWaitTimerActive)
        {
            choiceWaitTimer -= Time.deltaTime;

            if(choiceWaitTimer < 0f)
            {
                

                ChooseElementRandomly();
                
            }
        }
    }

    private void ChooseElementRandomly()
    {
        int randomElement = UnityEngine.Random.Range(0, 3);
       // Debug.Log(randomElement);
        DeactivateElementSelectionPanel(randomElement);
    }

    public void ActivateElementSelectionPanel(int elementTerrainId)
    {
        currTerrainElementId = elementTerrainId;
        //Time.timeScale = 0; //stop the player
        player.SetIsPlayerMoving(false);
        elemntSelectionPanel.SetActive(true);
        isChoiceWaitTimerActive = true;
    }

    public void DeactivateElementSelectionPanel(int elementSelectedId)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth

        isChoiceWaitTimerActive = false;
        choiceWaitTimer = choiceWaitTime;

        elemntSelectionPanel.SetActive(false);

        //Time.timeScale = 0;
        //also check if wrong fuel to kill player

        switch (currTerrainElementId)
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
        if (isPlayerAlive)
        {
            pickupSystem.ConsumeFuel(elementSelectedId);
            player.SetIsPlayerMoving(true);
        }

        //Time.timeScale = 1;    //enable player
       

        
       
    }

    private void Kill()
    {
        player.KillPlayer();
    }
}
