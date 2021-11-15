using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float mobileHorizontalRunSpeed = 2f;
    [SerializeField] float horizontalSpeed = 4f;

    private float gravity = -9.8f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private float horizontalInput;
    private float verticalInput;
    private float jumpHeight;
    private float jumpDistance;

    bool drag;
    bool isPlayerMoving = true;
    bool isTouchActive = false;
    bool isJump = false;

    Animator animator;
    GameSession gameSession;

    

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        characterController = GetComponent<CharacterController>();

        transform.position = gameSession.lastCheckPointPos;

       // transform.rotation = gameSession.lastCheckPointTransform.rotation;
        animator = GetComponent<Animator>();
        //this.animator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMoving)
        {

            verticalInput = 1;
            horizontalInput = Input.GetAxis("Horizontal");

            if (Input.touchCount > 0)
            {
                if (!isTouchActive)
                    isTouchActive = true;
                horizontalInput = Input.GetTouch(0).deltaPosition.x;
            }



            transform.forward = new Vector3(-verticalInput, 0, Mathf.Abs(verticalInput) - 1);


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

            if(isGrounded && isJump)
            {
                isJump = false;
                velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            }


            var movement = Vector3.zero;
            if(isTouchActive)
                 movement = new Vector3(verticalInput * runSpeed, 0, horizontalInput * -1 * mobileHorizontalRunSpeed);

            else
                 movement = new Vector3(verticalInput * runSpeed, 0, horizontalInput * -1 * horizontalSpeed);

            //Mathf.Clamp(movement.z, -1.5f, 1.5f);
            characterController.Move(movement * Time.deltaTime);
            characterController.Move(velocity * Time.deltaTime);
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                Mathf.Clamp(transform.position.z, -1.5f, 1.5f));
        }
    }

    public void KillPlayer()
    {
        animator.enabled = false;
        isPlayerMoving = false;
        
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.AddComponent<Rigidbody>();
        //characterController.
        Destroy(characterController);
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


}
