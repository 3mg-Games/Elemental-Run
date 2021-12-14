using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float mobileHorizontalRunSpeed = 2f;
    [SerializeField] float horizontalSpeed = 4f;
    [SerializeField] GameObject confetti;
    [SerializeField] float currentAngleDelta = 2f;
    [SerializeField] GameObject playerBurningVfx;
    //[SerializeField] float wallRunMaxDistance = 1f;
    //turn them off
    /*[SerializeField] CinemachineVirtualCamera northCam;
    [SerializeField] CinemachineVirtualCamera westCam;
    [SerializeField] CinemachineVirtualCamera eastCam;
    [SerializeField] CinemachineVirtualCamera southCam;*/

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
    }

    

    private void Move()
    {
        verticalInput = 1;
        horizontalInput = Input.GetAxis("Horizontal");

        if (mobileInput)
        {
            if (Input.touchCount > 0)
            {
                if (!isTouchActive)
                    isTouchActive = true;
                horizontalInput = Input.GetTouch(0).deltaPosition.x;
            }
        }


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

            if(!isJump)
            animator.SetBool("Jump", false);
        }

        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        // MovementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (isGrounded && isJump)
        {
            isJump = false;
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            animator.SetBool("Jump", true);
        }


        var movement = Vector3.zero;
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

        //code for clamping the left and right of player

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


            if (transform.position == targetPosition)
                wallRunWayPointsIdx++;

        }

        else
        {
            cam.SwitchToNormalCam();
            transform.rotation = Quaternion.Euler(originalRoationEulerAngles);
           
            isWallRun = false;
            mobileInput = true;
            isPlayerMoving = true;
        }
    
    }

    private void Turn()
    {
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


            if (transform.position == targetPosition)
                turnWayPointIdx++;

        }

        else
        {
            isTurn = false;
            isPlayerMoving = true;
        }
    }

    public void KillPlayer(bool isDeathByWater, int terrainID)
    {
        //animator.enabled = false;
        //isPlayerMoving = false;
        FindObjectOfType<CamerFollow>().SetParentNull();
        animator.enabled = true;   //enable animator for fuel empty condition
        if (!isDeathByWater)
            animator.SetTrigger("Trip");
        else
            animator.SetTrigger("Fall");
         isPlayerMoving = false;

        //SetIsPlayerMoving(false);
        transform.GetChild(2).gameObject.SetActive(false);
        if(!isDeathByWater && terrainID == 0)
        {
            StartCoroutine(BurnPlayer());
        }
        StartCoroutine(FallThrough(terrainID, isDeathByWater));
        
        //characterController.
        //Destroy(characterController);
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
        }
        if(terrainId == 1)
        {
            timeAfterFallThroughHappens = 1f;
        }
        yield return new WaitForSeconds(timeAfterFallThroughHappens);

        if (terrainId == 1 || terrainId == 0)   //if fire or water then sink the player
        {
            gameObject.AddComponent<Rigidbody>();
            Physics.gravity = new Vector3(0, -0.8f, 0);
            
        }
        characterController.enabled = false;
    }

    public void PlayerWin()
    {
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

    public void Jump(float jumpHeight, float jumpDistance)
    {
        this.jumpHeight = jumpHeight;
        this.jumpDistance = jumpDistance;
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
        var clampLimits = new Vector2(clampLowerLimit, clampUpperLimit);
        return clampLimits;
    }

    public void Slant(float percentageChangeInSpeed, bool isEntry)
    {
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
            animator.SetBool("Jump", false);

            mobileInput = false;
            //isWallRotate = true;
            isPlayerMoving = false;
            isWallRun = true;
        }
    }
}
