using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSfx;
    [SerializeField] [Range(0, 1f)] float pickUpSfxVolume = 1f;

    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickUpSfx,
                   Camera.main.transform.position,
                   pickUpSfxVolume);
            gameSession.IncrementCoin();
            Destroy(gameObject);

        }
    }
}
