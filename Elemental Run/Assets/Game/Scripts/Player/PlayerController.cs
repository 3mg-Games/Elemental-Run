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

    //turn them off
    /*[SerializeField] CinemachineVirtualCamera northCam;
    [SerializeField] CinemachineVirtualCamera westCam;
    [SerializeField] CinemachineVirtualCamera eastCam;
    [SerializeField] CinemachineVirtualCamera southCam;*/

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

    bool drag;
    bool isPlayerMoving = true;
    bool isTouchActive = false;
    bool isJump = false;
    bool isTurn = false;

    bool isNorth = true;
    bool isWest = false;
    bool isEast = false;
    bool isSouth = false;

    bool isClampZ;
    bool isClampX;

    Animator animator;
    GameSession gameSession;

    List<Transform> turnWaypoints;

    int turnWayPointIdx;
    int turnWayPointsCount;

    int camPriority = 2;

    float clampLowerLimit;
    float clampUpperLimit;

    private float initialRunSpeed;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        characterController = GetComponent<CharacterController>();

        transform.position = gameSession.lastCheckPointPos;
        dir = gameSession.playerDir;

        switch(dir)
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

       // transform.rotation = gameSession.lastCheckPointTransform.rotation;
        animator = GetComponent<Animator>();

        turnWaypoints = new List<Transform>();
        turnWayPointIdx = 0;

        initialRunSpeed = runSpeed;

        //this.animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMoving)
        {
            Move();
        }

        else if(isTurn)
        {
            Turn();
        }
    }

   

    private void Move()
    {
        verticalInput = 1;
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.touchCount > 0)
        {
            if (!isTouchActive)
                isTouchActive = true;
            horizontalInput = Input.GetTouch(0).deltaPosition.x;
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

    public void KillPlayer()
    {
        //animator.enabled = false;
        //isPlayerMoving = false;
        SetIsPlayerMoving(false);
        
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.AddComponent<Rigidbody>();
        characterController.enabled = false;
        //characterController.
        //Destroy(characterController);
    }

    public void PlayerWin()
    {
        animator.SetTrigger("Win");
        isPlayerMoving = false;
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
}
