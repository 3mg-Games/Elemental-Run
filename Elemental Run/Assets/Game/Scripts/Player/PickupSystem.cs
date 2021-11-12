using System.Collections;
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

    float[] elements = new float[3];     // 0 - fire
                                         // 1 - water
                                         // 2 - earth


    PlayerController player;
   // float startingCapacityOfContainers;
    GameSession gameSession;
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

        IncrementFire();
        IncrementWater();
        IncrementEarth();
    }

   
    // Update is called once per frame
    void Update()
    {
        
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
                IncrementFire();
                break;

            case 1:
                IncrementWater();
                break;

            case 2:
                IncrementEarth();
                break;
        }
    }

    void IncrementFire()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = fireFluid.localScale;
        fireFluid.localScale = new Vector3(scale.x, elements[0], scale.z);
       
    }

    void IncrementWater()
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        var scale = waterFluid.localScale;
        waterFluid.localScale = new Vector3(scale.x, elements[1], scale.z);
       
    }

    void IncrementEarth()
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
                ConsumeFire();
                break;

            case 1:
                ConsumeWater();
                break;

            case 2:
                ConsumeEarth();
                break;
        }
    }

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
    }

    public float[] GetElements()
    {
        return elements;
    }


}
