using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireClearing : MonoBehaviour
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
        //put fire tage on the fire vfxs or gameobjects that needs to be cleared
        
        if(other.tag == "Fire")
        {
            DestroyClearing(other.gameObject);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        
    }

    void DestroyClearing(GameObject fire)
    {
        Destroy(fire);
    }
}
