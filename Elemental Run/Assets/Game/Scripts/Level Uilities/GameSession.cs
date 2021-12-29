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
    // 3 - ice
    [Tooltip("Set it to true this to play a particular level in Unity Editor." +
        "\nFor building, make it false for all levels")]
    [SerializeField] bool isEditor = false;

    [Tooltip("Keep it true for all levels")]
    [SerializeField] bool test1 = true;

    [Tooltip("The elmenet selection Canvas")]
    [SerializeField] GameObject elemntSelectionPanel;

    //[Tooltip("North - ")]
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



   
    //[SerializeField] Transform coinTextImg;
    //[SerializeField] GameObject coinSpawnPrefab;
    //[SerializeField] GameObject coinSpawnCanvas;
    //[SerializeField] RectTransform localCoinPos;
    //[SerializeField] bool isLevel1 = true;
    [SerializeField] GameObject coinSpawnPrefab;
    [SerializeField] Transform coinTarget;


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

    bool changeLevelNum = false;

    bool isPlayerAlive = true;
    bool isChoiceWaitTimerActive = false;
    bool hasGameStarted = false;
    bool isFirstTimeTutorial = true;
    bool hasLevelLoaded = false;
    // public bool isNewLevel = false;

    GameObject mainCam;

    float choiceWaitTimer;
    float playerInputWaitTimer;

    LevelLoader levelLoader;
    private static GameSession instance;

    int choice;
    int coinCount;

    bool isRespawn = false;

    ProgressBar progressBar;
    private void Awake()
    {
        //Gamesession gets destroyed whenever a new level is loaded, regardless of the singleton pattern
        // it dosent get detroyed only when, player dies and same level is loaded
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            for (int i = 0; i < 3; i++)
            {
                lastElementsCapacity[i] = startingCapacityOfContainers;   //initializing the fluid level in player 
                                                                           //backpack
            }
            playerDir = 1;         
            clampLowerLimit = -1.5f;         //clamp left limit
            clampUpperLimit = 1.5f;         //clamp right limit
            isClampZ = true;
            isClampX = false;
           
           
            playerInputWaitTimer = waitTimeForPlayerInput;
            choice = twoChoiceSystemChoiceCount;

       //     if (isEditor)
            
            //Deleting keys related to Player Progress whenever new level is loaded
       
            PlayerPrefs.DeleteKey("Progress");

            PlayerPrefs.DeleteKey("PlayerDirection");

            PlayerPrefs.DeleteKey("Player Distance");

            PlayerPrefs.DeleteKey("PrevPlayerPosX");
            PlayerPrefs.DeleteKey("PrevPlayerPosY");
            PlayerPrefs.DeleteKey("PrevPlayerPosZ");

        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        progressBar = FindObjectOfType<ProgressBar>();
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        currLevelNum = FindObjectOfType<LevelNumber>().GetLevelNumber();
        mainCam = player.transform.GetChild(3).transform.gameObject;
        elemntSelectionPanel.SetActive(false);
        choiceWaitTimer = choiceWaitTime;
        //currLevelNum = levelLoader.GetCurrentSceneBuildIdx() + 1;
        //
        //PlayerPrefs.DeleteAll();
        

        
        int savedLevelNum = PlayerPrefs.GetInt("Level", 1);

        Debug.Log("Saved Level num = " + savedLevelNum);
        Debug.Log("Curr Level Num = " + currLevelNum);

        //if the level which is supposed to load hasnt loaded, then destroy the current 
        // gamesession object and load the particular scene
        if (currLevelNum != savedLevelNum && !isEditor)
        {
            levelLoader.LoadParticularScene(savedLevelNum-1);
            Destroy(gameObject);
        }

        //if (changeLevelNum)
        //{
            Debug.Log("Level number changed");
            int isRandomised = PlayerPrefs.GetInt("IsRandomized", 0);
            if (isRandomised == 1)
            {
                /*if levels are randomised i.e, once level 10 has been crossed
                * then whichever random level is loaded
                * update its number
                * Btw, every level has an object with a fixed level  number
                * In this script there are 2 variables for level number
                * 1 - "currLevelNum" - the deafult number of a level
                * 2 - "savedLevelNum" - the default level number which is supposed to load
                * */
                int currLevel = PlayerPrefs.GetInt("LevelCount", 1);  // "LevelCount" Player Prefs stores the true incremental count
                                                                      // regardless of the actual level number or randomization
                FindObjectOfType<LevelNumber>().SetLevelNumber(currLevel);
            }
        //}
        /*
        if (currLevelNum != 1)
        {
            hasLevelLoaded = false;
            Destroy(tutorial);
            hasGameStarted = true;
            player.SetIsPlayerMoving(true);
        }
        */
        if(savedLevelNum == 1)
        {
           PlayerPrefs.DeleteKey("Coins");  
        }
        coinCount = PlayerPrefs.GetInt("Coins", 0);
        coinText.text = coinCount.ToString();

        player.SetIsPlayerMoving(false);   //stop the player from moving at the beginning of every level


       // changeLevelNum = true;

        //coinImg = 

        //PlayerPrefs.SetInt("Level", currLevelNum);

        //PlayerPrefs.SetFloat("Progress", progressBar.ProgressBarFill);
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
                //if there is player input, start the game
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
                    //if choice panel is active and after a certian time
                    //player hasnt chosen anything, choose randomly automatically

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
        //choice panel gets activated the moment player touches a terrain

        player.ActivateSpeedVFx(false);
        currTerrainElementId = elementTerrainId;
        //Time.timeScale = 0; //stop the player
        player.SetIsPlayerMoving(false);
        elemntSelectionPanel.SetActive(true);
        if (is2Choices)
        {
            //code for when there are 2 choices, bascially for level 1 and 2
            // and for those 2 levels, which choices will appear at what poin
            //have been hard coded down below
            choice++;
            GameObject leftSideChoice = elemntSelectionPanel.transform.GetChild(0).gameObject;
            GameObject rightSideChoice = elemntSelectionPanel.transform.GetChild(1).gameObject;

            GameObject lFire = leftSideChoice.transform.GetChild(0).gameObject;
            GameObject lWater = leftSideChoice.transform.GetChild(1).gameObject;
            GameObject lEarth = leftSideChoice.transform.GetChild(2).gameObject;

            GameObject rFire = rightSideChoice.transform.GetChild(0).gameObject;
            GameObject rWater = rightSideChoice.transform.GetChild(1).gameObject;
            GameObject rEarth = rightSideChoice.transform.GetChild(2).gameObject;

            GameObject elementalTutorial = elemntSelectionPanel.transform.GetChild(3).gameObject;
            GameObject lElementalTutorial = elementalTutorial.transform.GetChild(0).gameObject; 
            GameObject rElementalTutorial = elementalTutorial.transform.GetChild(1).gameObject;

            lElementalTutorial.SetActive(false);
            rElementalTutorial.SetActive(false);

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

                case 1: //level 1
                    switch (choice)
                    {
                        case 1:  //first terrain
                            lFire.SetActive(true);
                            rEarth.SetActive(true);
                            lElementalTutorial.SetActive(true);
                            break;

                        case 2: //second terrain
                            //lEarth.SetActive(true);
                            lFire.SetActive(true);
                            rWater.SetActive(true);
                            rElementalTutorial.SetActive(true);
                            break;

                        case 3:  //third terrain
                            //lFire.SetActive(true);
                            lWater.SetActive(true);
                            rEarth.SetActive(true);
                            rElementalTutorial.SetActive(true);
                            break;
                    }

                    break;

                case 2:   //level 2
                    switch (choice)
                    {
                        case 1:   //first terrain
                            lEarth.SetActive(true);
                            //rFire.SetActive(true);
                            rWater.SetActive(true);
                            break;

                        case 2:   //second terrain
                            lFire.SetActive(true);
                            //rWater.SetActive(true);
                            rEarth.SetActive(true);
                            break;

                        case 3:  //third terrain

                            lWater.SetActive(true);
                            //rEarth.SetActive(true);
                            rFire.SetActive(true);
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

                    break;



            }

        
        }

        /*else if(is1Choice)
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
                * 2 - earth

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

                case 2:
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

                    break;



            }
        }*/
        isChoiceWaitTimerActive = true;
    }

    public void DeactivateElementSelectionPanel(int elementSelectedId)
    {
        //when a choice has been made, deactivte choice pannel
        // 0 - fire
        // 1 - water
        // 2 - earth
        // 3 - ice
        isChoiceWaitTimerActive = false;
        choiceWaitTimer = choiceWaitTime;

        elemntSelectionPanel.SetActive(false);

        //Time.timeScale = 0;
        //also check if wrong fuel to kill player

        //Debug.Log("Terrain ID = " + currTerrainElementId + " , Selected Element ID" + elementSelectedId);

        if (test1)
        { //code for test1, we are not doing test2 anymore, it was a previous thing

            switch (currTerrainElementId)
            {
                //if at a particluar terrain wrong choice is made,
                //kill player otherwise start consuming the fuel

                case 0: //fire
                    if (elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                      //  Debug.Log("Terrain id = " + currTerrainElementId +
                      //      ", Selected element id = " + elementSelectedId);
                        StartCoroutine(Kill(false, 0));
                    }

                    break;

                case 1: //water
                    if (elementSelectedId == 0)
                    {
                      //  Debug.Log("Terrain id = " + currTerrainElementId +
                       //     ", Selected element id = " + elementSelectedId);
                        isPlayerAlive = false;
                        StartCoroutine(Kill(false, 1));
                    }


                    break;

                case 2:   //earth
                    if (elementSelectedId == 1)
                    {
                       // Debug.Log("Terrain id = " + currTerrainElementId +
                       //     ", Selected element id = " + elementSelectedId);
                        isPlayerAlive = false;
                        StartCoroutine(Kill(false, 2));

                    }

                    break;


                case 3:   //ice
                    if (elementSelectedId == 1 || elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill(false, 3));
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
                        StartCoroutine(Kill(false, 0));
                    }

                    break;

                case 1: //water
                    if (elementSelectedId == 0 || elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill(false, 1));
                    }


                    break;

                case 2:   //earth
                    if (elementSelectedId == 0 || elementSelectedId == 1)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill(false, 2));

                    }

                    break;

                case 3:   //ice
                    if (elementSelectedId == 1 || elementSelectedId == 2)
                    {
                        isPlayerAlive = false;
                        StartCoroutine(Kill(false, 3));
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
            bool val = pickupSystem.ConsumeFuel(elementSelectedId, currTerrainElementId);
            //if player is alive i.e, he didt make wronng choice, start consuming fuel
            if(val)
            player.SetIsPlayerMoving(true);
        }

        //Time.timeScale = 1;    //enable player
       

        
       
    }

    public IEnumerator Kill(bool isDeathByWater, int terrainID)
    {
        //kill player

        isPlayerAlive = false;
        player.KillPlayer(isDeathByWater, terrainID);
        // isNewLevel = false;
        var timeAfterWhichSceneIsLoaded = 3.5f;
        if (terrainID == 3)
            timeAfterWhichSceneIsLoaded = 4.5f;

        //store the correct level count
        int currLevel = PlayerPrefs.GetInt("LevelCount", currLevelNum);
        
        //send the msg to game analytics sdk, of failing the level
        FindObjectOfType<Immortal>().LevelFail(currLevel);

        yield return new WaitForSeconds(timeAfterWhichSceneIsLoaded);
        isRespawn = true;
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
        //this is the part, that i dont recommend u to touch, coz honestly
        //even i dont know how it works
        //touch it at your own risk

        //when level is loaded again after failing
        //below are lines of code to initialize stuffa and get refernces of 
        //objects in case they are missing
        pickupSystem = FindObjectOfType<PickupSystem>();
        player = FindObjectOfType<PlayerController>();
        levelLoader = FindObjectOfType<LevelLoader>();
        mainCam = player.transform.GetChild(3).transform.gameObject;
        GameObject canvas = GameObject.FindGameObjectWithTag("Selection Canvas").gameObject;
        //coinSpawnCanvas = GameObject.FindGameObjectWithTag("Coin Canvas").gameObject;                   //commented out
        //localCoinPos = coinSpawnCanvas.transform.GetChild(0).gameObject.GetComponent<RectTransform>();  // commented out
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
      //  coinTextImg = canvas.transform.GetChild(4).                                                    //commented out
       //     gameObject.transform.GetChild(0).transform;                                                 //commented out

        coinTarget = Camera.main.transform.GetChild(2).transform;

        isChoiceWaitTimerActive = false;
        choiceWaitTimer = choiceWaitTime;
        isPlayerAlive = true;

        // currLevelNum = levelLoader.GetCurrentSceneBuildIdx() + 1;
        currLevelNum = FindObjectOfType<LevelNumber>().GetLevelNumber();
        //choice = 0;
        /*
        for (int i = 0; i < 3; i++)
        {
            lastElementsCapacity[i] = startingCapacityOfContainers;
        }*/

        hasLevelLoaded = true;

        if(currLevelNum > 2)
        {
            is2Choices = false;
        }

        if(currLevelNum > 0)
        {
            is1Choice = false;
        }

        hasLevelLoaded = false;
        //Destroy(tutorial);
        //hasGameStarted = true;
        hasGameStarted = false;
        player.SetIsPlayerMoving(true);

        choice = twoChoiceSystemChoiceCount;

        if (isRespawn)
        {

            Debug.Log("Level number changed");
            int isRandomised = PlayerPrefs.GetInt("IsRandomized", 0);
            if (isRandomised == 1)
            {
                int currLevel = PlayerPrefs.GetInt("LevelCount", 1);
                FindObjectOfType<LevelNumber>().SetLevelNumber(currLevel);
            }
        }

        if(isRespawn)
        {
            hasGameStarted = true;
        }
        /*int isRandomised = PlayerPrefs.GetInt("IsRandomized", 0);
        if (isRandomised == 1)
        {
            int currLevel = PlayerPrefs.GetInt("LevelCount", 1);
            FindObjectOfType<LevelNumber>().SetLevelNumber(currLevel);
        }*?

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
        PlayerPrefs.DeleteKey("Progress");
      
        PlayerPrefs.DeleteKey("PlayerDirection");
        
        PlayerPrefs.DeleteKey("Player Distance");
        PlayerPrefs.DeleteKey("PrevPlayerPosX");
        PlayerPrefs.DeleteKey("PrevPlayerPosY");
        PlayerPrefs.DeleteKey("PrevPlayerPosZ");

        
        pickupSystem.SetBonus(false);

        AudioSource.PlayClipAtPoint(levelCompleteSfx,
            Camera.main.transform.position,
            levelCompleteSfxVolume);
        player.PlayerWin();


        mainCam.GetComponent<CamerFollow>().ActivateRotate();
        int currLevel = PlayerPrefs.GetInt("LevelCount", currLevelNum);
        FindObjectOfType<Immortal>().LevelComplete(currLevel);

        pickupSystem.DeactivateSmokeVfx();
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
        int currLevel = PlayerPrefs.GetInt("LevelCount", currLevelNum);
        Debug.Log("Curr Level = " + currLevel);
        PlayerPrefs.SetInt("LevelCount", currLevel + 1);
         currLevel = PlayerPrefs.GetInt("LevelCount", currLevelNum);
        /*if (PlayerPrefs.GetInt("Level") > levelLoader.GetTotalSceneCount())
        {
              //randomize here
            PlayerPrefs.SetInt("IsRandomized", 1);
            int randomLevel = UnityEngine.Random.Range(3, 11);
            PlayerPrefs.SetInt("Level", randomLevel);
            levelLoader.LoadNextScene();
        }

        else
        {
            levelLoader.LoadNextScene();
        }
        */

        if (currLevel > 10)
        {
            PlayerPrefs.SetInt("IsRandomized", 1);
           // int randomLevel = UnityEngine.Random.Range(3, 11);
            //

            //PlayerPrefs.SetInt("Level", randomLevel);
            // levelLoader.LoadNextScene();
            levelLoader.LoadNextScene(true);
            //levelLoader.LoadNextScene();
        }


        else
        {
            levelLoader.LoadNextScene(false);
        }
        
        Destroy(gameObject);
    }

    public void IncrementCoin(Transform coinTransform)
    {
        var pos = Vector3.zero;
        var dir = player.GetDir();
        if (dir == 1)
        pos = player.transform.position + new Vector3(-1f, 1.6f, 0f);

        else if(dir == 2)
            pos = player.transform.position + new Vector3(0f, 1.6f, -1f);
        GameObject coinSpawn = Instantiate(coinSpawnPrefab.gameObject,
            pos,
            player.transform.rotation)
            as GameObject; //coinSpawnCanvas.transform.GetChild(0).
                                                                           //GetComponent<RectTransform>().anchoredPosition,
       // coinSpawn.transform.SetParent(coinSpawnCanvas.transform);
        //coinSpawn.transform.localPosition = localCoinPos.localPosition;
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
        //return coinTextImg;
        return coinTarget;
    }

    /*
    public bool GetIsNewLevel()
    {
        
       return isNewLevel;
    }*/

}
