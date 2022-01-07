using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterClearing : MonoBehaviour
{
    bool isWaterClearing = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isWaterClearing)
        {
            CreateVineBridge();
        }
    }

    //though on trigger might not come in handy for tackling water clearing
    //so i am creating another function for that too to clear water
    //and that will be called from outside this script when player enter water terrain
    private void OnTriggerEnter(Collider other)
    {
        //put water tag on the water vfxs or gameobjects that needs to be cleared

        if (other.tag == "Water")
        {
            // DestroyClearing(other.gameObject);
            //create vines to cross water
            CreateVineBridge();
        }
    }



    private void CreateVineBridge()
    {
       //code for creating vine bridge
    }

    private void OnTriggerStay(Collider other)
    {

    }

    void DestroyClearing(GameObject water)
    {
        Destroy(water);
    }


    public void StartWaterClearing(bool val)
    {
        isWaterClearing = val;
    }
}
