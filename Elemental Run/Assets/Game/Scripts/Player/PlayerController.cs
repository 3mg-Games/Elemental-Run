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

    bool drag;
    bool isPlayerAlive = true;
    bool isTouchActive = false;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerAlive)
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
        isPlayerAlive = false;
        Destroy(characterController);
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.AddComponent<Rigidbody>();
    }
}
