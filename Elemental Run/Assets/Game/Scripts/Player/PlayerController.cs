using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //[Header("")]
    
    [SerializeField] LayerMask groundLayer;

    [Tooltip("Forward Running spped")]
    [SerializeField] float runSpeed = 8f;

    [Tooltip("Mobile horizontal swerve speed")]
    [SerializeField] float mobileHorizontalRunSpeed = 2f;

    [Tooltip("Unity Editor horizontal swerve speed")]
    [SerializeField] float horizontalSpeed = 4f;
    [SerializeField] GameObject confetti;
    [SerializeField] float currentAngleDelta = 2f;
    [SerializeField] GameObject playerBurningVfx;
    [SerializeField] GameObject speedVfx;
    [SerializeField] GameObject mainCam;
    [SerializeField] AudioClip sinkingSfx;
    [SerializeField] AudioClip lavaSinkingSfx;
    [SerializeField] AudioClip iceFreezingSfx;
    [SerializeField] AudioClip grassFallSfx;
    [SerializeField] AudioClip punchSfx;
    [SerializeField] Material freezingMaterial;
    [SerializeField] SkinnedMeshRenderer renderer;
    [SerializeField] GameObject coldFumes;
    [SerializeField] GameObject punchVfx;
    [SerializeField] GameObject confusedVfx;
    [SerializeField] GameObject floorHitVfx;

    [Tooltip("Camera for bonus level jumping which zooms out and looks player from above.")]
    [SerializeField] CinemachineVirtualCamera bonusLevelCam;

    //[SerializeField] float wallRunMaxDistance = 1f;
    //turn them off
    /*[SerializeField] CinemachineVirtualCamera northCam;
    [SerializeField] CinemachineVirtualCamera westCam;
    [SerializeField] CinemachineVirtualCamera eastCam;
    [SerializeField] CinemachineVirtualCamera southCam;*/

    //direction of player
    // forward - north
    // backward - south
    // left - west
    // right - east
    [Tooltip("North - 1, West - 2, East - 3, South - 4")]
    public int dir;

    private float gravity = -12.8f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private float horizontalInput;
    private float verticalInput;
    private float jumpHeight;
    private float jumpDistance;
    private float turnMovementSpeed, turnRotateSpeed;
    private float wallRunSpeed;


    bool drag;
    bool isPlayerMoving = true;
    bool isTouchActive = false;
    bool isJump = false;
    bool isTurn = false;
    bool isWallRun = false;

    bool isNorth = true;
    bool isWest = false;
    bool isEast = false;
    bool isSouth = false;

    bool isClampZ;
    bool isClampX;

    bool isWallRotate = false;

    bool mobileInput = true;
    bool isBonusMidAir = false;
    Animator animator;
    public GameSession gameSession;

    List<Transform> turnWaypoints;
    List<Transform> wallRunWayPoints;

    int turnWayPointIdx;
    int turnWayPointsCount;

    int wallRunWayPointsIdx;
    int wallRunWayPointsCount;

    int camPriority = 2;

    int wallPos = 0;

    float clampLowerLimit;
    float clampUpperLimit;
   

    private float initialRunSpeed;

    Vector3 originalRoationEulerAngles;
    Quaternion targetRotation;
    Quaternion curRotation;
    [SerializeField] CamerFollow cam;

    //[SerializeField] Color normal
    bool isBonusJump;
    AudioSource audioSource;


    float currHeight;
    private void Awake()
    {
       // ResetPlayer();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        SetIsPlayerMoving(false);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetPlayer();
        audioSource = GetComponent<AudioSource>();
        //cam = Camera.main.GetComponent<CamerFollow>();
        // transform.rotation = gameSession.lastCheckPointTransform.rotation;


        turnWaypoints = new List<Transform>();
        wallRunWayPoints = new List<Transform>();
        turnWayPointIdx = 0;

        initialRunSpeed = runSpeed;

        // SetIsPlayerMoving(false);
        //this.animator.enabled = true;
    }

    private void ResetPlayer()
    {
        //initialize player properties on spawning
        //like player position and player clamp limist for left and right swerve
        
        gameSession = (GameSession)FindObjectOfType(typeof(GameSession));
        //if (!gameSession.GetIsNewLevel())
       // {
            Debug.Log("Game session last checkpoint pos = " + gameSession.lastCheckPointPos.x + ", "+
                gameSession.lastCheckPointPos.y + ", " +
                gameSession.lastCheckPointPos.z);
            
            transform.position = gameSession.lastCheckPointPos;

            Debug.Log("Player Position = " + transform.position.x + ", " +
                transform.position.y + ", "+
                transform.position.z + ", ");
       // }
        dir = gameSession.playerDir;

        //set player direction based on direction that was saved on last checkpoint
        //if its the level begining it sets that to north automatically

        /* 1 - North
         * 2 - West
         * 3 - East
         * 4 - South
         * P.S. Code for south doesnt exist */
         
        switch (dir)
        {
            case 1:
                isNorth = true;
                isEast = false;
                isWest = false;
                isSouth = false;

                break;

            case 2:
                isNorth = false;
                isEast = false;
                isWest = true;
                isSouth = false;

                //westCam.Priority = westCam.Priority + ++camPriority;
                break;

            case 3:
                isNorth = false;
                isEast = true;
                isWest = false;
                isSouth = false;
                break;

            case 4:
                isNorth = false;
                isEast = false;
                isWest = false;
                isSouth = true;
                break;
        }

        isClampZ = gameSession.isClampZ;
        isClampX = gameSession.isClampX;
        clampLowerLimit = gameSession.clampLowerLimit;
        clampUpperLimit = gameSession.clampUpperLimit;
    }

    // Update is called once per frame
    void Update()
    {
        /*Debug.Log("Before load Player Position = " + transform.position.x + ", " +
               transform.position.y + ", " +
               transform.position.z + ", ");
        if (gameSession == null)
            ResetPlayer();

        Debug.Log("After load Player Position = " + transform.position.x + ", " +
               transform.position.y + ", " +
               transform.position.z + ", ");*/
        // gameSession = (GameSession)FindObjectOfType(typeof(GameSession));


        if (isPlayerMoving)
        {
            Move();
        }

        else if(isTurn)
        {
            Turn();
        }

        else if(isWallRun)
        {
            WallRun();
        }

        if(isWallRotate)
        {
            WallRunPlayerRoate();
        }

        if(isBonusMidAir)
        {
            BonusMidAir();
        }
    }

    

    private void Move()
    {
        verticalInput = 1;
        horizontalInput = Input.GetAxis("Horizontal");

        if (mobileInput)
        {
            //if the game is being run on mobile then
            //the code automatically swithces to the speed variables for mobile
            if (Input.touchCount > 0)
            {
                if (!isTouchActive)
                    isTouchActive = true;
                horizontalInput = Input.GetTouch(0).deltaPosition.x;
            }
        }

        // based on player direction, move player in a certain direction only
        if(isNorth)
            transform.forward = new Vector3(-verticalInput, 0, Mathf.Abs(verticalInput) - 1);

        //code for west
        if(isWest)
            transform.forward = new Vector3(Mathf.Abs(verticalInput) - 1, 0, -verticalInput);

        if(isEast)
            transform.forward = new Vector3(Mathf.Abs(verticalInput) - 1, 0, verticalInput);

        
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer, QueryTriggerInteraction.Ignore);

        if (isGrounded && velocity.y < 0)
        {

            velocity.y = 0;

            if (!isJump)
            {
                ActivateSpeedVFx(false);
                animator.SetBool("Jump", false);
            }
        }

        //if player is not grounded, start making him fall using gravity
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        // MovementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (isGrounded && isJump)
        {
            isJump = false;
            if(isBonusJump)
            {
                //if player is jumping in bonus area, then do so in a specific way,
                //ohterwise do so, in a diffeerent way
                velocity.y += Mathf.Sqrt(jumpHeight * -jumpDistance * gravity);
            }

            else
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetBool("Jump", true);
        }


        var movement = Vector3.zero;


        //if game is being run on mobile, use one set of speed variables
        //otherweise use second set
        if (isTouchActive)
        {
            if(isNorth)
                movement = new Vector3(verticalInput * runSpeed, 0, horizontalInput * -1 * mobileHorizontalRunSpeed);

            else if (isWest)
                movement = new Vector3(horizontalInput * mobileHorizontalRunSpeed, 0, verticalInput * runSpeed);

            else if (isEast)
                movement = new Vector3(horizontalInput * -1 * mobileHorizontalRunSpeed, 0, -verticalInput * runSpeed);
        }

        else
        {
            if(isNorth)
                movement = new Vector3(verticalInput * runSpeed, 0, horizontalInput * -1 * horizontalSpeed);

            else if(isWest)
                movement = new Vector3(horizontalInput * horizontalSpeed, 0, verticalInput * runSpeed);

            else if(isEast)
                movement = new Vector3(horizontalInput * -1* horizontalSpeed, 0, -verticalInput * runSpeed);
        }



        //Mathf.Clamp(movement.z, -1.5f, 1.5f);
        characterController.Move(movement * Time.deltaTime);
        characterController.Move(velocity * Time.deltaTime);

        //code for clamping the left and right i.e, horizontal swerve movement of player

        if (isClampZ)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                Mathf.Clamp(transform.position.z, clampLowerLimit, clampUpperLimit));
                //Mathf.Clamp(transform.position.z, -1.5f, 1.5f));
        }

        else if(isClampX)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, clampLowerLimit, clampUpperLimit),
                transform.position.y,
                transform.position.z);
        }
    }

    private void WallRunPlayerRoate()
    {
        //This code is for a older implementaltion of wall running mechanic

        curRotation = transform.rotation;
        //= transform.rotation;
        //if (smooth)
        // {
        // currentAngleDelta = SmoothDelay(currentAngleDelta, angle, smoothDelay);
        //}
        //else
        //{

        //}
        if (wallPos == 1)
        {
            curRotation *= Quaternion.Euler(0f, 0f, currentAngleDelta);
            if (curRotation.eulerAngles.z >= targetRotation.eulerAngles.z)
            {
                isWallRotate = false;
                return;
            }
        }

        else

        {
            curRotation *= Quaternion.Euler(0f, 0f, -currentAngleDelta);
            if (curRotation.eulerAngles.z <= targetRotation.eulerAngles.z)
            {
                isWallRotate = false;
                return;
            }
        }

        // Debug.Log("Curr rot z = " + curRotation.eulerAngles.z + " Target Rot z = " + targetRotation.eulerAngles.z);

       
        
        transform.rotation = curRotation;
    }

    private void WallRun()
    {
       //code for wall running of player
       /* the way it works is -
        * waypoints are already place over the wall
        * the momeent player touches the wall
        * wall run anim is trigereed
        * and player is moved from one waypoint to the other
        * at the same height player touche the wall
        * */

      // Debug.Log(Camera.main.transform.rotation.eulerAngles);
        if (wallRunWayPointsIdx <= wallRunWayPointsCount - 1)
        {
           //Debug.Log(transform.rotation.eulerAngles);
            Vector3 dir;
            var targetOriginalPosition = wallRunWayPoints[wallRunWayPointsIdx].transform.position;

            //var targetPosition = targetOriginalPosition;
            
            var targetPosition = new Vector3(targetOriginalPosition.x,
                transform.position.y,
                targetOriginalPosition.z);

            var movementThisFrame = wallRunSpeed * Time.deltaTime;
            //var rotationThisFrame = turnRotateSpeed * Time.deltaTime;
            dir = targetPosition - transform.position;
           // Quaternion rotation = Quaternion.LookRotation(-dir, Vector3.up);
            //Debug.Log(rotation);

            transform.position = Vector3.MoveTowards        //try character controller here
                      (transform.position, targetPosition, movementThisFrame);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationThisFrame);

            //if player has reached the target waypoint
            //change target waypoint to the next one 
            if (transform.position == targetPosition)
                wallRunWayPointsIdx++;

        }

        //if player has reached the final waypoint
        //switch to normal movement

        else
        {
            cam.SwitchToNormalCam();
            transform.rotation = Quaternion.Euler(originalRoationEulerAngles);
            ActivateSpeedVFx(false);
            animator.SetBool("Jump", false);  //check this line later
            animator.SetBool("WallRun", false);
            isWallRun = false;
            mobileInput = true;
            isPlayerMoving = true;
        }
    
    }

    private void Turn()
    {
        //code for making player turn 
        /* it also works on waypoint concepty
         * the moment player touches the turn
         * he is automatically moved throught the turn
         * by waypoints */
        if (turnWayPointIdx <= turnWayPointsCount - 1)
        {
            Vector3 dir;

            //verticalInput = 1;
            //transform.forward = new Vector3(-verticalInput, 0, Mathf.Abs(verticalInput) - 1);

            var targetPosition = turnWaypoints[turnWayPointIdx].transform.position;

            var movementThisFrame = turnMovementSpeed * Time.deltaTime;
            var rotationThisFrame = turnRotateSpeed * Time.deltaTime;
            dir = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(-dir, Vector3.up);
            //Debug.Log(rotation);

            transform.position = Vector3.MoveTowards        //try character controller here
                      (transform.position, targetPosition, movementThisFrame);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationThisFrame);


            // if player has reached the target waypoint
            // make the next waypoint as the target one
            if (transform.position == targetPosition)
                turnWayPointIdx++;

        }

        //if plyaer has reached the final waypoint
        //resume normal movement
        else
        {
            ActivateSpeedVFx(false);
            isTurn = false;
            isPlayerMoving = true;
        }
    }

    public void KillPlayer(bool isDeathByWater, int terrainID)
    {
        //code for killing the player

        //animator.enabled = false;
        //isPlayerMoving = false;

        //Deactivate speed vfx
        ActivateSpeedVFx(false);

        mainCam.GetComponent<CamerFollow>().SetParentNull();
        animator.enabled = true;   //enable animator for fuel empty condition
        if (!isDeathByWater && terrainID != 3 && terrainID != 4 &&
            terrainID != 2)  //not ice and not death by water and not death by boxing glove and not earth terrain
        {
            animator.SetBool("Jump", false);
            animator.SetTrigger("Trip");
        }

        else if(terrainID == 2)
        {
            animator.SetBool("Jump", false);
            animator.SetTrigger("Swallow");
        }


        else if(terrainID == 3)
        {
            //if death by ice terrain
            animator.SetBool("Jump", false);
            animator.SetTrigger("Freeze");

            audioSource.clip = iceFreezingSfx;
            audioSource.loop = false;
            audioSource.Play();
            StartCoroutine(Freeze());
        }

        else if(terrainID == 4)
        {
            //if death by boxing gloves
            animator.SetBool("Jump", false);
            animator.SetTrigger("Knock");
            punchVfx.SetActive(true);
            confusedVfx.SetActive(true);
            

            StartCoroutine(FloorHit());
        }

        else
            animator.SetTrigger("Fall");

        isPlayerMoving = false;

        //SetIsPlayerMoving(false);

        //deactivate backpack
        transform.GetChild(2).gameObject.SetActive(false);

        if(!isDeathByWater && terrainID == 0)
        {
            //if death by fire terrain
            StartCoroutine(BurnPlayer());
        }

        //activate falling thorugh of player
        StartCoroutine(FallThrough(terrainID, isDeathByWater));
        
        //characterController.
        //Destroy(characterController);
    }

    private IEnumerator FloorHit()
    {
        yield return new WaitForSeconds(0.4f);

        floorHitVfx.SetActive(true);
    }

    private IEnumerator Freeze()
    {
        var freezeWaitTime = 1.8f;

        yield return new WaitForSeconds(freezeWaitTime);

        Material[] mats = renderer.materials;
        mats[1] = freezingMaterial;
        renderer.materials = mats;

        coldFumes.SetActive(true);
    }

    private IEnumerator BurnPlayer()
    {
        yield return new WaitForSeconds(1f);
        playerBurningVfx.SetActive(true);
    }

    private IEnumerator FallThrough(int terrainId, bool isDeathByWater)
    {
        var timeAfterFallThroughHappens = 2f;

        if(isDeathByWater)
        {
            gameObject.AddComponent<Rigidbody>();
            characterController.enabled = false;
            Physics.gravity = new Vector3(0f, -14f, 0f);
            audioSource.clip = sinkingSfx;
            //audioSource.loop = false;
            audioSource.Play();
        }

        if(terrainId == 1)
        {
            //if water terrain
            timeAfterFallThroughHappens = 1f;
        }

        else if(terrainId == 3)
        {
            //if ice terrain
            timeAfterFallThroughHappens = 3f;
        }

        else if (terrainId == 2)
        {
            //if earth terrain
            StartCoroutine(PlayGrassFallSfx());
        }


        else if(terrainId == 4)
        {
            //if punching gloves
            audioSource.clip = punchSfx;
            audioSource.loop = false;
            audioSource.Play();
            //audioSource.PlayOneShot(punchSfx);
            
        }
        yield return new WaitForSeconds(timeAfterFallThroughHappens);

        if (terrainId == 1 || terrainId == 0 || terrainId == 3)   //if fire or water or ice then sink the player
        {
            if(terrainId == 1 || terrainId == 3)
            {//if water or ice
                audioSource.clip = sinkingSfx;
            }

            else if(terrainId == 0)
            { 
                //if fire
                audioSource.clip = lavaSinkingSfx;
                audioSource.loop = false;
            }
            //audioSource.clip = sinkingSfx;
            //audioSource.loop = false;
            audioSource.Play();
            gameObject.AddComponent<Rigidbody>();
            Physics.gravity = new Vector3(0, -0.8f, 0);
            
        }

        

        characterController.enabled = false;
    }

    private IEnumerator PlayGrassFallSfx()
    {
        yield return new WaitForSeconds(0.3f);

        audioSource.clip = grassFallSfx;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayerWin()
    {
        //acitvate wining stuff
        animator.SetTrigger("Win");
        isPlayerMoving = false;
        confetti.SetActive(true);
    }

    public void SetIsPlayerMoving(bool val)
    {
        isPlayerMoving = val;
        if(!val)
        {
            animator.enabled = false;
        }

        else
        {
            animator.enabled = true;
        }
    }

    private void BonusMidAir()
    {
        //something was used for an earlier implemntation fo bonus area
        // currently not in use
        if(currHeight < transform.position.y)
        {
            currHeight = transform.position.y;
        }

        else if(currHeight > transform.position.y)
        {
            currHeight = transform.position.y;
            ActivateSpeedVFx(true);
            isBonusMidAir = false;
        }
    }

    public void DeactivateBonusLevelCam()
    {
        bonusLevelCam.Priority = 1;
    }

    public void Jump(float jumpHeight, float jumpDistance, bool isJumpVfx, bool isBonusJump)
    {
        //acitvate plyaer jump

        this.jumpHeight = jumpHeight;
        this.jumpDistance = jumpDistance;

        if(isJumpVfx)
            ActivateSpeedVFx(true);

        this.isBonusJump = isBonusJump;

        if(isBonusJump)
        {
            bonusLevelCam.Priority = 100;
           // gravity = bonusGravity;
        }

        currHeight = transform.position.y;
        //isBonusMidAir = true;

        
        isJump = true;
        // animator.SetBool("Jump", true);
       // animator.SetBool("Jump", true);

    }

    /*
    private void Rotate()
    {
        //  if(controller.transform.rotation.y < 90)
        rotateSpeed += 5f;
        rotateSpeed = Mathf.Clamp(rotateSpeed, 0f, 90f);
        controller.transform.rotation = Quaternion.Euler(0f, -rotateSpeed, 0f);
        if (rotateSpeed >= 90f)
        {
            rotate = false;
            hasRotated = true;
        }
    }*/

    public void SpeedTurn(float turnMovementSpeed, float turnRotateSpeed, GameObject path, int turnDirection)
    {

        //activate turning and also set the
        //new player direction after turning
        // and also teh speed and rotation of turning
        // and the path of waypoints to follow
        /* North - 1
         * West - 2
         * East - 3 
         * South - 4*/

        //aslo get the direction of turn
        isPlayerMoving = false;

        switch(turnDirection)
        {
            case 1:
                isNorth = true;
                isEast = false;
                isWest = false;
                isSouth = false;

                break;

            case 2:
                isNorth = false;
                isEast = false;
                isWest = true;
                isSouth = false;

                //westCam.Priority = westCam.Priority + ++camPriority;
                break;

            case 3:
                isNorth = false;
                isEast = true;
                isWest = false;
                isSouth = false;
                break;

            case 4:
                isNorth = false;
                isEast = false;
                isWest = false;
                isSouth = true;
                break;
        }

        SetDir(turnDirection);
        turnWaypoints.Clear();
        for(int i = 0; i < path.transform.childCount; i++)
        {
           //Debug.Log(path.transform.childCount);
          //  Debug.Log(path.transform.GetChild(i).gameObject.transform.position);
            turnWaypoints.Add(path.transform.GetChild(i).gameObject.transform);
        }

        this.turnMovementSpeed = turnMovementSpeed;
        this.turnRotateSpeed = turnRotateSpeed;

        turnWayPointIdx = 0;
        turnWayPointsCount = path.transform.childCount;

        ActivateSpeedVFx(true);
        isTurn = true;
      
    }

    void SetDir(int direction)
    {
        dir = direction;
    }

    public int GetDir()
    {
        return dir;
    }

    public void SetClamp(float clampLowerLimit, 
        float clampUpperLimit, bool isClampX, bool isClampZ)
    {
        //set new clamps for player which will limit the
        // horizontal swerve movemnet fo the player at any path
        this.clampLowerLimit = clampLowerLimit;
        this.clampUpperLimit = clampUpperLimit;
        this.isClampX = isClampX;
        this.isClampZ = isClampZ;

        //Debug.Log(this.clampLowerLimit + " , " + this.clampUpperLimit + " , " + this.isClampX + " , " + this.isClampZ);
    }

    public bool GetIsClampX()
    {
        return isClampX;
    }

    public bool GetIsClampZ()
    {
        return isClampZ;
    }

    public Vector2 GetClampLimits()
    {
        //return the current clamp limits
        var clampLimits = new Vector2(clampLowerLimit, clampUpperLimit);
        return clampLimits;
    }

    public void Slant(float percentageChangeInSpeed, bool isEntry)
    {
        //eartlier implementation
        //currently not in use
        if(isEntry)
        {
            runSpeed = runSpeed + initialRunSpeed * percentageChangeInSpeed / 100f;
        }

        else
        {
            runSpeed = runSpeed - initialRunSpeed * percentageChangeInSpeed / 100f;
        }
    }


    public void ActivateWallRun(bool val, GameObject path, float wallRunSpeed, int wallPos)
    {
        //activate wall run and initalizet things like - 
        // wall run speed, path of waypoints to follow while wall running
        // and also the the position of wall - left or right w.r.t. player's perspective
        if(val)
        {

            wallRunWayPoints.Clear();
            for (int i = 0; i < path.transform.childCount; i++)
            {
                //Debug.Log(path.transform.childCount);
                //  Debug.Log(path.transform.GetChild(i).gameObject.transform.position);
                wallRunWayPoints.Add(path.transform.GetChild(i).gameObject.transform);
            }

            this.wallRunSpeed = wallRunSpeed;
            wallRunWayPointsCount = path.transform.childCount;
            //this.turnRotateSpeed = turnRotateSpeed;

            //check player is closest to which waypoint
            var closestWayPoint = wallRunWayPoints[0];
            var closestDistance = Vector3.Distance(transform.position, closestWayPoint.position);
            wallRunWayPointsIdx = 0;
            int ind;
            for (ind = 0; ind < wallRunWayPointsCount; ind++)
            {
                var currWaypoint = wallRunWayPoints[ind];
                if (dir == 1)
                {
                    if (closestWayPoint.transform.position.x <= transform.position.x)
                    {
                        closestWayPoint = wallRunWayPoints[ind];
                        closestDistance = Vector3.Distance(transform.position, currWaypoint.position);
                        wallRunWayPointsIdx = ind;

                    }

                    else
                    {
                        break;
                    }
                }

                else if(dir == 2)
                {
                    if (closestWayPoint.transform.position.z <= transform.position.z)
                    {
                        closestWayPoint = wallRunWayPoints[ind];
                        closestDistance = Vector3.Distance(transform.position, currWaypoint.position);
                        wallRunWayPointsIdx = ind;

                    }

                    else
                    {
                        break;
                    }
                }
            }

            var curWaypoint = wallRunWayPoints[ind];
            closestWayPoint = wallRunWayPoints[ind];
            closestDistance = Vector3.Distance(transform.position, curWaypoint.position);
            wallRunWayPointsIdx = ind;

            for (int i = ind+1; i < wallRunWayPointsCount; i++)
            {
                var currWaypoint = wallRunWayPoints[i];
                if (Vector3.Distance(transform.position, currWaypoint.position) < closestDistance)
                {
                    closestWayPoint = wallRunWayPoints[i];
                    closestDistance = Vector3.Distance(transform.position, currWaypoint.position);
                    wallRunWayPointsIdx = i;
                }
            }


            // wallRunWayPointsIdx = 0;
            this.wallPos = wallPos;
            cam.SwitchToWallRunCam(this.wallPos, dir);
            originalRoationEulerAngles = transform.rotation.eulerAngles;
            //Debug.Log(Camera.main.transform.rotation.eulerAngles);
            if(wallPos == 1)
             //  transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 34.9f));
             targetRotation = Quaternion.Euler(new Vector3(0f, 270f, 34.9f)); 

            else
            {
               // transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, -34.9f));
               targetRotation = Quaternion.Euler(new Vector3(0f, 270f, -34.9f)); 
            }

            ActivateSpeedVFx(true);
            animator.SetBool("WallRun", true);
            
            mobileInput = false;
            //isWallRotate = true;
            isPlayerMoving = false;
            isWallRun = true;
        }
    }

    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
    }

    public void ActivateSpeedVFx(bool val)
    {
        speedVfx.SetActive(val);
    }

    public void Dash()
    {
        animator.SetBool("Jump", false);
        animator.SetTrigger("Dash");
    }
}
