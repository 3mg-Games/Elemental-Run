using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for when player enters touches the wall
public class WallRunEnter : MonoBehaviour
{
    /* 1 - Left
     * 2 - Right*/
    [Tooltip("1 - Left side, 2 - Right side")]
    [SerializeField] int wallPos;

    [SerializeField] GameObject path;
    [SerializeField] float wallRunSpeed;
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
        if(other.tag == "Player")
        {
            player.ActivateWallRun(true, path, wallRunSpeed, wallPos);
            //do stuff for tilting the player
        }
    }
}
