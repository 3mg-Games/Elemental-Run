using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalEnemy : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [Tooltip("Fire - 0, Water - 1, Earth - 2")]
    [SerializeField] int elementId = 0;
    [Tooltip("Fire - 0, Water - 1, Earth - 2")]
    [SerializeField] int counterElementNeededId = 0;

    [SerializeField] GameObject explosionVfxPrefab;

    PickupSystem pickupSystem;
    // Start is called before the first frame update
    void Start()
    {
        pickupSystem = FindObjectOfType<PickupSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pickupSystem.ElementalEnemyCheck(elementId, counterElementNeededId, gameObject);

        }
    }

    public void Destroy()
    {
        GameObject explosionVfx = Instantiate(explosionVfxPrefab, transform.position, Quaternion.identity);
        Destroy(explosionVfx, 1.5f);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
