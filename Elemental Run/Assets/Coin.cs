using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] AudioClip pickUpSfx;
    [SerializeField] [Range(0, 1f)] float pickUpSfxVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(pickUpSfx,
                   Camera.main.transform.position,
                   pickUpSfxVolume);

            Destroy(gameObject);

        }
    }
}
