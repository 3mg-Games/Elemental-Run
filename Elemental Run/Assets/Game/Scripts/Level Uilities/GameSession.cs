using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [SerializeField] bool isEditor = false;
    [SerializeField] bool test1 = true;
    [SerializeField] GameObject elemntSelectionPanel;
    [SerializeField] float choiceWaitTime = 10f;
    [SerializeField] float startingCapacityOfContainers = 0.02f;
    [SerializeField] GameObject winScreen;
    [SerializeField] bool is2Choices = false;
    [SerializeField] bool is1Choice = false;
    [SerializeField] int currLevelNum;
    [SerializeField] AudioClip levelCompleteSfx;
    [SerializeField] [Range(0, 1)] float levelCompleteSfxVolume = 1f;
    [SerializeField] float waitTimeForPlayerInput = 5f;
    [SerializeField] GameObject tutorial;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Transform coinTextImg;
    [SerializeField] GameObject coinSpawnPrefab;
    [SerializeField] GameObject coinSpawnCanvas;
    [SerializeField] RectTransform localCoinPos;
    //[SerializeField] bool isLevel1 = true;

    public PlayerController player;
    public Vector3 lastCheckPointPos;
    public float[] lastElementsCapacity = new float[3];
    [Tooltip("North - 1, West - 2, East - 3, South - 4")]
    public int playerDir;
    public int twoChoiceSystemChoiceCount = 0;

    public float clampLowerLimit;
    public float clampUpperLimit;
    public bool isClampZ;
    public bool isClampX;

    PickupSystem pickupSystem;

    public int currTerrainElementId;

   

    bool isPlayerAlive = true;
    bool isChoiceWaitTimerActive = false;
    bool hasGameStarted = false;
    bool isFirstTimeTutorial = true;
    bool hasLevelLoaded = false;
   // public bool isNewLevel = false;

        

    float choiceWaitTimer;
    float playerInputWaitTimer;

    LevelLoader levelLoader;
    private static GameSession instance;

    int choice;
    int coinCount;
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
           
           
            playerInputWaitTimer = waitTimeForPlayerInput;
            choice = twoChoiceSystemChoiceCount;
            

            
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        currLevelNum = FindObjectOfType<LevelNumber>().GetLevelNumber();
        elemntSelectionPanel.SetActive(false);
        choiceWaitTimer = choiceWaitTime;
        //currLevelNum = levelLoader.GetCurrentSceneBuildIdx() + 1;
        PlayerPrefs.DeleteAll();

        int savedLevelNum = PlayerPrefs.GetInt("Level", 1);
        if (currLevelNum != savedLevelNum && !isEditor)
        {
            levelLoader.LoadParticularScene(savedLevelNum-1);
            Destroy(gameObject);
        }
        if (currLevelNum != 1)
        {
            hasLevelLoaded = false;
            Destroy(tutorial);
            hasGameStarted = true;
            player.SetIsPlayerMoving(true);
        }
        if(savedLevelNum == 1)
        {
           PlayerPrefs.DeleteKey("Coins");
        }
        coinCount = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coinCount.ToString();
        //coinImg = 

        //PlayerPrefs.SetInt("Level", currLevelNum);
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = coinCount.ToString();
        if (hasLevelLoaded)
        {
            hasLevelLoaded = false;
            if (isFirstTimeTutorial)
            {
                player.SetIsPlayerMoving(false);
            }

            else
            {
                Destroy(tutorial);
                hasGameStarted = true;
                player.SetIsPlayerMoving(true);
            }
        }

        if(!hasGameStarted && isFirstTimeTutorial)
        {
            playerInputWaitTimer -= Time.deltaTime;
            if(playerInputWaitTimer <= 0f || Input.GetMouseButtonDown(0))
            {
                Destroy(tutorial);
                isFirstTimeTutorial = false;
                hasGameStarted = true;
                player.SetIsPlayerMoving(true);
            }

            if(playerInputWaitTimer <= waitTimeForPlayerInput - 2)
            {
                tutorial.SetActive(true);
            }
        }
        if (hasGameStarted)
        {
            if (isChoiceWaitTimerActive)
            {
                choiceWaitTimer -= Time.deltaTime;

                if (choiceWaitTimer < 0f)
                {


                    ChooseElementRandomly();

                }
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
        if (is2Choices)
        {
            choice++;
            GameObject leftSideChoice = elemntSelectionPanel.transform.GetChild(0).gameObject;
            GameObject rightSideChoice = elemntSelectionPanel.transform.GetChild(1).gameObject;

            GameObject lFire = leftSideChoice.transform.GetChild(0).gameObject;
            GameObject lWater = leftSideChoice.transform.GetChild(1).gameObject;
            GameObject lEarth = leftSideChoice.transform.GetChild(2).gameObject;

            GameObject rFire = rightSideChoice.transform.GetChild(0).gameObject;
            GameObject rWater = rightSideChoice.transform.GetChild(1).gameObject;
            GameObject rEarth = rightSideChoice.transform.GetChild(2).gameObject;

            lFire.SetActive(false);
            lWater.SetActive(false);
            lEarth.SetActive(false);

            rFire.SetActive(false);
            rWater.SetActive(false);
            rEarth.SetActive(false);
            

            switch (currLevelNum)
            {
                /*children of each side - 
                * 0 - fire
                * 1 - water
                * 2 - earth*/

                case 2:
                    switch (choice)
                    {
                        case 1:
                            lFire.SetActive(true);
                            rWater.SetActive(true);
                            break;

                        case 2:
                            lEarth.SetActive(true);
                            rWater.SetActive(true);
                            break;

                        case 3:
                            lFire.SetActive(true);
                            rEarth.SetActive(true);
                            break;
                    }

                    break;

                case 3:
                    switch (choice)
                    {
                        case 1:
                            lEarth.SetActive(true);
                            rFire.SetActive(true);
                            break;

                        case 2:
                            lFire.SetActive(true);
                            rWater.SetActive(true);
                            break;

                        case 3:
                            lWater.SetActive(true);
                            rEarth.SetActive(true);
                            break;
                    }

                    break;

                case 4:
                    switch (choice)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:
                            break;
                    }

                    break;



            }

        
        }

        else if(is1Choice)
        {
            choice++;
            GameObject centerChoices = elemntSelectionPanel.transform.GetChild(0).gameObject;
            

            GameObject cFire = centerChoices.transform.GetChild(0).gameObject;
            GameObject cWater = centerChoices.transform.GetChild(1).gameObject;
            GameObject cEarth = centerChoices.transform.GetChild(2).gameObject;

            

            cFire.SetActive(false);
            cWater.SetActive(false);
            cEarth.SetActive(false);

           

            switch (currLevelNum)
            {
                /*children of each side - 
                * 0 - fire
                * 1 - water
                * 2 - earth*/

                case 1:
                    switch (choice)
                    {
                        case 1:
                            cFire.SetActive(true);
                            
                            break;

                        case 2:
                            
                            cWater.SetActive(true);
                            break;

                        case 3:
                            
                            cEarth.SetActive(true);
                            break;
                    }

                    break;

               /* case 2:
                    switch (choice)
                    {
                        case 1:
                           
                            break;

                        case 2:
                           
                            break;

                        case 3:

                            break;
                    }

                    break;

                case 3:
                    switch (choice)
                    {
                        case 1:

                            break;
                        case 2:

                            break;
                        case 3:
                            break;
                    }

                    break;*/



            }
        }
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

        //Debug.Log("Terrain ID = " + currTerrainElementId + " , Selected Element ID" + elementSelectedId);

        if (test1)
        {

            switch (currTerrainElementId)
            {

                case 0: //fire
                    if (elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                      //  Debug.Log("Terrain id = " + currTerrainElementId +
                      //      ", Selected element id = " + elementSelectedId);
                        StartCoroutine(Kill());
                    }

                    break;

                case 1: //water
                    if (elementSelectedId == 0)
                    {
                      //  Debug.Log("Terrain id = " + currTerrainElementId +
                       //     ", Selected element id = " + elementSelectedId);
                        isPlayerAlive = false;
                        StartCoroutine(Kill());
                    }


                    break;

                case 2:   //earth
                    if (elementSelectedId == 1)
                    {
                       // Debug.Log("Terrain id = " + currTerrainElementId +
                       //     ", Selected element id = " + elementSelectedId);
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
            bool val = pickupSystem.ConsumeFuel(elementSelectedId);
            if(val)
            player.SetIsPlayerMoving(true);
        }

        //Time.timeScale = 1;    //enable player
       

        
       
    }

    public IEnumerator Kill()
    {
        isPlayerAlive = false;
        player.KillPlayer();
       // isNewLevel = false;
        yield return new WaitForSeconds(2f);
        
        levelLoader.LoadCurrentScene();
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();

        if (currLevelNum != 1)
        {
            hasLevelLoaded = false;
            Destroy(tutorial);
            hasGameStarted = true;
            player.SetIsPlayerMoving(true);
        }
    }

    

    private void OnLevelWasLoaded()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();

        GameObject canvas = GameObject.FindGameObjectWithTag("Selection Canvas").gameObject;
        coinSpawnCanvas = GameObject.FindGameObjectWithTag("Coin Canvas").gameObject;
        localCoinPos = coinSpawnCanvas.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        elemntSelectionPanel = canvas.transform.GetChild(0).gameObject;
        elemntSelectionPanel.SetActive(false);
        

        winScreen = canvas.transform.GetChild(2).gameObject;
        winScreen.SetActive(false);

        tutorial = canvas.transform.GetChild(3).gameObject;

        coinText = canvas.transform.GetChild(4).
            gameObject.transform.GetChild(1).
            gameObject.GetComponent<TextMeshProUGUI>();
        coinText.text = coinCount.ToString();                  //edit coin code here
        //coinSpawnTarget = Camera.main.transform.GetChild(0).transform;
        coinTextImg = canvas.transform.GetChild(4).
            gameObject.transform.GetChild(0).transform;

        isChoiceWaitTimerActive = false;
        choiceWaitTimer = choiceWaitTime;
        isPlayerAlive = true;

        // currLevelNum = levelLoader.GetCurrentSceneBuildIdx() + 1;
        currLevelNum = FindObjectOfType<LevelNumber>().GetLevelNumber();
        //choice = 0;
        
        for (int i = 0; i < 3; i++)
        {
            lastElementsCapacity[i] = startingCapacityOfContainers;
        }

        hasLevelLoaded = true;

        if(currLevelNum > 3)
        {
            is2Choices = false;
        }

        if(currLevelNum > 1)
        {
            is1Choice = false;
        }

        hasLevelLoaded = false;
        Destroy(tutorial);
        hasGameStarted = true;
        player.SetIsPlayerMoving(true);

        choice = twoChoiceSystemChoiceCount;

      /*  if(isNewLevel)
        {
            for (int i = 0; i < 3; i++)
            {
                lastElementsCapacity[i] = startingCapacityOfContainers;
            }
            playerDir = 1;
            clampLowerLimit = -1.5f;
            clampUpperLimit = 1.5f;
            isClampZ = true;
            isClampX = false;
            // coinCount = 0;
            //  playerInputWaitTimer = waitTimeForPlayerInput;
            choice = twoChoiceSystemChoiceCount = 0;
            lastCheckPointPos = new Vector3(0f, 0.15f, 0f);
        }*/
        //playerDir = 1;
        Debug.Log("LEvel was loaded");
        //hasGameStarted = true;
        //player.SetIsPlayerMoving(true);
    }

    public void Win()
    {
        pickupSystem.SetBonus(false);

        AudioSource.PlayClipAtPoint(levelCompleteSfx,
            Camera.main.transform.position,
            levelCompleteSfxVolume);
        player.PlayerWin();


        FindObjectOfType<CamerFollow>().ActivateRotate();
        StartCoroutine(AcitvateWinScreen());
    }

    private IEnumerator AcitvateWinScreen()
    {
        yield return new WaitForSeconds(1f);
        
        winScreen.SetActive(true);
    }

    public void Continue()
    {
        //levelLoader.LoadScene(0);
        //isNewLevel = true;
        PlayerPrefs.SetInt("Coins", coinCount);

        PlayerPrefs.SetInt("Level", currLevelNum + 1);
        levelLoader.LoadNextScene();
        
        Destroy(gameObject);
    }

    public void IncrementCoin()
    {
        var pos = player.transform.position + new Vector3(0, 2f, 0);
        GameObject coinSpawn = Instantiate(coinSpawnPrefab) as GameObject; //coinSpawnCanvas.transform.GetChild(0).
                                                                           //GetComponent<RectTransform>().anchoredPosition,
        coinSpawn.transform.SetParent(coinSpawnCanvas.transform);
        coinSpawn.transform.localPosition = localCoinPos.localPosition;
            //Quaternion.identity,
           // coinSpawnCanvas.transform);
        //coinSpawn.transform.SetParent(coinSpawnCanvas.transform, false);
       // coinCount++;
        //if(coinCount > PlayerPrefs.GetInt("Coins", 0))
      //  {
            //PlayerPrefs.SetInt("Coins", coinCount);
           // coinText.text = coinCount.ToString();
        //}
        
    }

    public void RealCoinInc()
    {
        coinCount++;
        coinText.text = coinCount.ToString();
    }

    public void SetTwoChoiceCount()
    {
        twoChoiceSystemChoiceCount = choice;
    }

    private void OnApplicationQuit()
    {
       // PlayerPrefs.DeleteKey("Coins");
    }

    public Transform GetCoinSpawnTarget()
    {
        return coinTextImg;
    }

    /*
    public bool GetIsNewLevel()
    {
        
       return isNewLevel;
    }*/

}
