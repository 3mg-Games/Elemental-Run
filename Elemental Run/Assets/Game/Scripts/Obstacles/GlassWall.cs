using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassWall : MonoBehaviour
{
    [SerializeField] GameObject glassWallBrokenPrefab;
    [SerializeField] float timeAfterWhichPiecesDisappear = 1.5f;
    [SerializeField] float explosionForce = 5f;
    [SerializeField] float explosionRadius = 5f;

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
            Destroy(gameObject);
        }
    }
}
