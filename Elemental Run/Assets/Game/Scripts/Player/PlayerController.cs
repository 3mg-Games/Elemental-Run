using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float horizontalSpeed = 4f;

    private float gravity = -9.8f;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private float horizontalInput;
    private float verticalInput;

    bool drag;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalInput = 1;
        horizontalInput = Input.GetAxis("Horizontal");




        transform.forward = new Vector3(-verticalInput, 0, Mathf.Abs(verticalInput) - 1);
        

        isGrounded = Physics.CheckSphere(transform.position, 0.1f, groundLayer, QueryTriggerInteraction.Ignore);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        // MovementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


        var movement = new Vector3(verticalInput * runSpeed, 0, horizontalInput * -1 * horizontalSpeed);
        //Mathf.Clamp(movement.z, -1.5f, 1.5f);
        characterController.Move(movement * Time.deltaTime);
        characterController.Move(velocity * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -1.5f, 1.5f));
    }
}
