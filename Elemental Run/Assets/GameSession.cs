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
    [SerializeField] float startingCapacityOfContainers = 0.02f;

    PickupSystem pickupSystem;
    public PlayerController player;

    int currTerrainElementId;

    bool isPlayerAlive = true;
    bool isChoiceWaitTimerActive = false;

    float choiceWaitTimer;
    LevelLoader levelLoader;
    private static GameSession instance;
    public Vector3 lastCheckPointPos;
    public float[] lastElementsCapacity = new float[3];
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            for (int i = 0; i < 3; i++)
            {
                lastElementsCapacity[i] = startingCapacityOfContainers;
            }
        }

        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        elemntSelectionPanel.SetActive(false);
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
                    StartCoroutine(Kill());
                }

                break;

            case 1: //water
                if (elementSelectedId == 0)
                {
                    isPlayerAlive = false;
                    StartCoroutine(Kill());
                }

                break;

            case 2:   //earth
                if (elementSelectedId == 1)
                {
                    isPlayerAlive = false;
                    StartCoroutine(Kill());
                    
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

    private IEnumerator Kill()
    {
        player.KillPlayer();

        yield return new WaitForSeconds(2f);

        levelLoader.LoadScene(0);
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    private void OnLevelWasLoaded()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        elemntSelectionPanel = GameObject.FindGameObjectWithTag("Selection Canvas").transform.GetChild(0).gameObject;
        elemntSelectionPanel.SetActive(false);
        isChoiceWaitTimerActive = false;
        choiceWaitTimer = choiceWaitTime;
    }
}
