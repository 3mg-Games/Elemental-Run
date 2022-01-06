using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthClearing : MonoBehaviour
{
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
        //put Earth tage on the fire vfxs or gameobjects that needs to be cleared

        if (other.tag == "Earth")
        {
            DestroyClearing(other.gameObject);
        }
    }


    private void OnTriggerStay(Collider other)
    {

    }

    void DestroyClearing(GameObject earth)
    {
        Destroy(earth);
    }
}
