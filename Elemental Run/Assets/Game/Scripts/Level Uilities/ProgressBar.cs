using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//code for manging progress bar
public class ProgressBar : MonoBehaviour
{
    [SerializeField] float maxDistance;
    //[SerializeField] 

    PlayerController player;
    Image progressBar;
    float playerDistance;
    Vector3 prevPlayerPos;
    public int playerDir = 1;
    float basePlayerDist;
    float lastDistance;
   
    bool isProgressFill = false;
    // Start is called before the first frame update
    void Start()
    {
       
        player = FindObjectOfType<PlayerController>();
        progressBar = GetComponent<Image>();

        //assuming player always starts from north
        //get last checkpoint player position and if its level start then, initailaize to 0
        Vector3 lastPlayerPos = new Vector3(
            PlayerPrefs.GetFloat("PrevPlayerPosX", 0),
            PlayerPrefs.GetFloat("PrevPlayerPosY", 0),
            PlayerPrefs.GetFloat("PrevPlayerPosZ", 0));
        
        //prevPlayerPos = player.transform.position;
        prevPlayerPos = lastPlayerPos;
        var initialPlayerDistance = prevPlayerPos.x;
      //  var initialPlayerDistance = PlayerPrefs.GetFloat("PrevPlayerPosX", 0);


        playerDistance = PlayerPrefs.GetFloat("Player Distance", initialPlayerDistance);
        basePlayerDist = playerDistance;
        var initialVal = playerDistance / maxDistance;
        progressBar.fillAmount = PlayerPrefs.GetFloat("Progress", initialVal);
       // Debug.Log("Load Initial val of progress = " + progressBar.fillAmount);
        //playerDir = gameSession.playerDir;
       
        //set player direction and other vairables
        ChangeDir(PlayerPrefs.GetInt("PlayerDirection", 1), lastPlayerPos);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isProgressFill && progressBar.fillAmount < 1)
        {
            //honestly even i dont remember how this calculation works 
            //touching this code isnt recommended

            //Debug.Log("Player distance = " + playerDistance);
            // int playerDir = player.GetDir();
            var diff = Vector3.zero;
            switch (playerDir)
            {
                case 1: //north
                   // Debug.Log("-------Calculation----------");
                    //Debug.Log("=======Difference===========");
                    //Debug.Log("Player Pos = " + player.transform.position);
                    //Debug.Log("Previous Player Pos = " + prevPlayerPos);
                    diff = player.transform.position - prevPlayerPos;
                    //diff = player.transform.position - basePlayerDist;
                    //
                    //Debug.Log("Difference = " + diff);

                    //Debug.Log("=======Player Distance=======");
                    //Debug.Log("Base Player distance = " + basePlayerDist);
                    //Debug.Log("Difference X = " + diff.x);
                    playerDistance = basePlayerDist + diff.x;
                   // Debug.Log("Player Distance = " + playerDistance);

                    //Debug.Log("=======Progress Bar===========");
                    //Debug.Log("Player Distance = " + playerDistance);
                    //Debug.Log("Max Distance = " + maxDistance);
                    progressBar.fillAmount = playerDistance / maxDistance;
                    //Debug.Log("Progress Bar Fill Amount = " + ProgressBarFill);

                    break;

                case 2: //west
                    diff = player.transform.position - prevPlayerPos;
                    playerDistance = basePlayerDist + diff.z;
                    progressBar.fillAmount = playerDistance / maxDistance;
                    break;

                case 3: //east
                    diff = player.transform.position  - prevPlayerPos;
                    //float var = diff.z;
                    

                    playerDistance = basePlayerDist + diff.z * -1;
                    progressBar.fillAmount = playerDistance / maxDistance;
                    break;
            }
            
        }
    }

    public float ProgressBarFill
    {
        get
        {
            return progressBar.fillAmount;
        }

        set
        {
            progressBar.fillAmount = value;
        }
    }

    public float PlayerDistance
    {
        get
        {
            return playerDistance;
        }

        set
        {
            playerDistance = value;
        }
    }

    public Vector3 PrevPlayerPos
    {
        get
        {
            return player.transform.position;
        }

        set
        {
            prevPlayerPos = value;
        }
    }

    public void EnableProgressFill(bool val)
    {
        isProgressFill = val;
    }
    


    public void ChangeDir(int dir, Vector3 pos)
    {
        Debug.Log("Direction and other parameters updated");
        basePlayerDist = playerDistance;
        prevPlayerPos = pos;
        playerDir = dir;

        EnableProgressFill(true);
        /*switch(dir)
        {
            case 1:
                basePlayerDist = playerDistance;
                prevPlayerPos = pos;
                playerDir = dir;
                break;

            case 2:
                basePlayerDist = playerDistance;
                prevPlayerPos = pos;
                playerDir = dir;
                break;


            case 3:
                basePlayerDist = playerDistance;
                prevPlayerPos = pos;
                playerDir = dir;
                break;
        }*/
    }


}
