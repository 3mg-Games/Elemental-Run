using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [SerializeField] bool test1 = true;
    [SerializeField] GameObject elemntSelectionPanel;
    [SerializeField] float choiceWaitTime = 10f;
    [SerializeField] float startingCapacityOfContainers = 0.02f;
    [SerializeField] GameObject conitnueButton;

    public PlayerController player;
    public Vector3 lastCheckPointPos;
    public float[] lastElementsCapacity = new float[3];
    public int playerDir;
    public float clampLowerLimit;
    public float clampUpperLimit;
    public bool isClampZ;
    public bool isClampX;

    PickupSystem pickupSystem;
    

    int currTerrainElementId;

    bool isPlayerAlive = true;
    bool isChoiceWaitTimerActive = false;

    float choiceWaitTimer;
    LevelLoader levelLoader;
    private static GameSession instance;
    
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
            playerDir = 1;
            clampLowerLimit = -1.5f;
            clampUpperLimit = 1.5f;
            isClampZ = true;
            isClampX = false;
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

        if (test1)
        {

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
        }

        else
        {
            switch (currTerrainElementId)
            {

                case 0: //fire
                    if (elementSelectedId == 1 || elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill());
                    }

                    break;

                case 1: //water
                    if (elementSelectedId == 0 || elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill());
                    }


                    break;

                case 2:   //earth
                    if (elementSelectedId == 0 || elementSelectedId == 1)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill());

                    }

                    break;

            }
        }

        //code for decreasing fuel level
        if (isPlayerAlive)
        {
          //  Debug.Log()
            if(currTerrainElementId == 1)
            {
                //Debug.Log(elementSelectedId);
            }
            pickupSystem.ConsumeFuel(elementSelectedId);
            player.SetIsPlayerMoving(true);
        }

        //Time.timeScale = 1;    //enable player
       

        
       
    }

    public IEnumerator Kill()
    {
        isPlayerAlive = false;
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
        isPlayerAlive = true;
    }

    public void Win()
    {
        conitnueButton.SetActive(true);
    }

    public void Continue()
    {
        levelLoader.LoadScene(0);
        Destroy(gameObject);
    }

    
}
