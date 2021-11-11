using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] float percentageIncreaseInElement = 0.1f;
    [SerializeField] Transform fireFluid;
    [SerializeField] Transform waterFluid;
    [SerializeField] Transform earthFluid;

    float[] elements = new float[3];     // 0 - fire
                                     // 1 - water
                                     // 2 - earth

    // Start is called before the first frame update
    void Start()
    {
       for(int i = 0; i < 3; i++)
        {
            elements[i] = 0.0001f;
        }

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

        switch(elementId)
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


}
