using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPad : MonoBehaviour
{
    /* North - 1
        * West - 2
        * East - 3 
        * South - 4*/
    [SerializeField] float turnMovementSpeed = 5f;
    [SerializeField] float turnRotateSpeed = 10f;
    [SerializeField] GameObject turnPath;
   // [Tooltip("Health value between 0 and 100.")]
    [Tooltip("North - 1, West - 2, East - 3, South - 4")]
    [SerializeField] int turnDirection = 1;

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Turn");
            player.SpeedTurn(turnMovementSpeed, turnRotateSpeed, turnPath, turnDirection);
        }
    }
}
