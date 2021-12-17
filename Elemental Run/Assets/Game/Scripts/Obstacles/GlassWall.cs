using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassWall : MonoBehaviour
{
    [SerializeField] GameObject glassWallBrokenPrefab;
    [SerializeField] GameObject glassOriginal;
    [SerializeField] float timeAfterWhichPiecesDisappear = 1.5f;
    [SerializeField] float explosionForce = 5f;
    [SerializeField] float explosionRadius = 5f;
   // [SerializeField] AudioClip glassBreakSfx;
   //[SerializeField] [Range(0f, 1f)] float glassBreakSfxVolume = 1f;

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
            Destroy(glassOriginal);
            GameObject glassWallBroken = Instantiate(glassWallBrokenPrefab,
                transform.position,
                transform.rotation);

            foreach(Transform child in glassWallBroken.transform)
            {
                Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(explosionForce,
                    player.transform.position,
                    explosionRadius);
                Destroy(child.gameObject, timeAfterWhichPiecesDisappear);
            }

            
            //Destroy(gameObject);
        }
    }
}
