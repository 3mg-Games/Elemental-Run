using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for when coin pickup is touched
public class Coin : MonoBehaviour
{
    [SerializeField] GameObject pickupVfxPrefab;
    [SerializeField] AudioClip pickUpSfx;
    [SerializeField] [Range(0, 1f)] float pickUpSfxVolume = 1f;

    GameSession gameSession;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<PlayerController>();
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
            var pos = player.transform.position + new Vector3(0, 0.7f, 0f);
            GameObject pickupVfx = Instantiate(pickupVfxPrefab,
             pos,
              Quaternion.identity, player.transform);
            Destroy(pickupVfx, 0.7f);

            AudioSource.PlayClipAtPoint(pickUpSfx,
                   Camera.main.transform.position,
                   pickUpSfxVolume);
            gameSession.IncrementCoin(gameObject.transform);
            Destroy(gameObject);

        }
    }
}
