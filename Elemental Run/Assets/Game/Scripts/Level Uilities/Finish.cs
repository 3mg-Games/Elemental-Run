using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    GameSession gameSession;
    PlayerController player;
    bool isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession == null)
            gameSession = FindObjectOfType<GameSession>();

        if(isWin)
        {
            if(player.IsGrounded)
            {
                // player.DeactivateBonusLevelCam();
                player.ActivateSpeedVFx(false);
                gameSession.Win();
                isWin = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isWin = true;

            GetComponent<Collider>().enabled = false;
        }
    }
}
