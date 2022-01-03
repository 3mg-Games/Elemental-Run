using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for player jump
public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpDistance = 2f;
    [SerializeField] bool isJumpVfx = true;
    [SerializeField] bool isBonusLevel = false;
    //[SerializeField] float bonusGrvity = -16f;
    PlayerController player;
    PickupSystem pickupSystem;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        pickupSystem = FindObjectOfType<PickupSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // map a value from one range to another
    private float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Jump");

            //if is bonus level then jump in a certain way otherwise do it another way


            if (isBonusLevel)
            {
                
                var elements = pickupSystem.GetElements();

                float totalFuel = elements[0] + elements[1] + elements[2];

                Debug.Log("Total Fuel = " + totalFuel);

                var newJumpVals = map(totalFuel, 0f, 3f, 12.5f, 52.5f);

                //player.Jump(jumpHeight, jumpDistance, isJumpVfx, isBonusLevel);
                player.Jump(newJumpVals, jumpDistance, isJumpVfx, isBonusLevel);
            }
            else

                player.Jump(jumpHeight, jumpDistance, isJumpVfx, isBonusLevel);

        }
    }
}
