using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraToNormal : MonoBehaviour
{
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
            player.DeactivateBonusLevelCam();

        }


    }

}
