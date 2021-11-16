﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] float percentageIncreaseInElement = 0.1f;
    [SerializeField] float percentageConsumptionInElement = 0.25f;
    [SerializeField] float lowerLimitOfContainers = 0.0001f;
    [SerializeField] float upperLimitOfContainers = 1f;
    //[SerializeField] float startingCapacityOfContainers = 0.1f;
    [SerializeField] Transform fireFluid;
    [SerializeField] Transform waterFluid;
    [SerializeField] Transform earthFluid;

    [SerializeField] TrailRenderer fireTerrainSpray;
    [SerializeField] TrailRenderer waterTerrainSpray;
    [SerializeField] TrailRenderer earthTerrainSpray;

    [SerializeField] Vector3 deltaTerrainSprayPos;

   

    float[] elements = new float[3];     // 0 - fire
                                         // 1 - water
                                         // 2 - earth


    PlayerController player;
   // float startingCapacityOfContainers;
    GameSession gameSession;

    bool isFireTerrainSpray = false;
    bool isWaterTerrainSpray = false;
    bool isEarthTerrainSpray = false;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gameSession = FindObjectOfType<GameSession>();

        /*
        startingCapacityOfContainers = gameSession
        */
        for (int i = 0; i < 3; i++)
        {
            elements[i] = gameSession.lastElementsCapacity[i];
        }

        //elements = gameSession.lastElementsCapacity;

        SetFire();
        SetWater();
        SetEarth();
        ResetAllTerrainSpray();

        
    }

   
    // Update is called once per frame
    void Update()
    {
        if(isFireTerrainSpray)
        {
            fireTerrainSpray.gameObject.transform.position = deltaTerrainSprayPos +
                transform.position;
        }

        if(isWaterTerrainSpray)
        {
            waterTerrainSpray.gameObject.transform.position = deltaTerrainSprayPos +
                transform.position;
        }

        if(isEarthTerrainSpray)
        {
            earthTerrainSpray.gameObject.transform.position = deltaTerrainSprayPos +
                transform.position;
        }
    }

    public void AddNewElementPickup(int elementId)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        elements[elementId] += percentageIncreaseInElement;

        if(elements[elementId] > upperLimitOfContainers)
        {
            elements[elementId] = upperLimitOfContainers;
        }

        
        switch (elementId)
        {
            case 0:
                SetFire();
                break;

            case 1:
                SetWater();
                break;

            case 2:
                SetEarth();
                break;
        }
    }

    void SetFire()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = fireFluid.localScale;
        fireFluid.localScale = new Vector3(scale.x, elements[0], scale.z);
       
    }

    void SetWater()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = waterFluid.localScale;
        waterFluid.localScale = new Vector3(scale.x, elements[1], scale.z);
       
    }

    void SetEarth()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = earthFluid.localScale;
        earthFluid.localScale = new Vector3(scale.x, elements[2], scale.z);
       
    }

    public void ConsumeFuel(int elementId)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        //Debug.Log("Before Decrement = " + elements[elementId]);
        elements[elementId] = elements[elementId] - percentageConsumptionInElement;
        //Debug.Log("Decrement = " + percentageConsumptionInElement);
        //Debug.Log("After Decrement = " + elements[elementId]);
        if (elements[elementId] < lowerLimitOfContainers)
        {
            elements[elementId] = lowerLimitOfContainers;
            //Debug.Log("empty container");
            player.KillPlayer();
        }
        switch(elementId)
        {
            case 0:
                isFireTerrainSpray = true;
                isWaterTerrainSpray = false;
                isEarthTerrainSpray = false;

                SetFire();
                ActivateFireTerrainSpray(true);
                
                break;

            case 1:
                isFireTerrainSpray = false;
                isWaterTerrainSpray = true;
                isEarthTerrainSpray = false;

                SetWater();
                ActivateWaterTerrainSpray(true);

                break;

            case 2:
                isFireTerrainSpray = false;
                isWaterTerrainSpray = false;
                isEarthTerrainSpray = true;

                SetEarth();
                ActivateEarthTerrainSpray(true);

                break;
        }
    }

    void ActivateFireTerrainSpray(bool val)
    {
        fireTerrainSpray.gameObject.SetActive(val);
        
        if(!val)
        {
            fireTerrainSpray.Clear();
            isFireTerrainSpray = false;
            
        }
    }

    void ActivateWaterTerrainSpray(bool val)
    {
        waterTerrainSpray.gameObject.SetActive(val);

        if (!val)
        {
            waterTerrainSpray.Clear();
            
            isWaterTerrainSpray = false;
            
        }
    }

    void ActivateEarthTerrainSpray(bool val)
    {
        earthTerrainSpray.gameObject.SetActive(val);

        if (!val)
        {
            earthTerrainSpray.Clear();
            
            isEarthTerrainSpray = false;
        }
    }

   

    public void ResetAllTerrainSpray()
    {
        //code for reseting all terrain sprays and disabling them
        ActivateFireTerrainSpray(false);
        ActivateWaterTerrainSpray(false);
        ActivateEarthTerrainSpray(false);
    }


    /*
    void ConsumeFire()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = fireFluid.localScale;
        fireFluid.localScale = new Vector3(scale.x, elements[0], scale.z);
    }

    void ConsumeWater()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = waterFluid.localScale;
        waterFluid.localScale = new Vector3(scale.x, elements[1], scale.z);
    }

    void ConsumeEarth()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = earthFluid.localScale;
        earthFluid.localScale = new Vector3(scale.x, elements[2], scale.z);
    }*/

    public float[] GetElements()
    {
        return elements;
    }


}