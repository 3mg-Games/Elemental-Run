﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSystem : MonoBehaviour
{
    [SerializeField] float percentageIncreaseInElement = 0.1f;
    [SerializeField] float percentageConsumptionInElement = 0.25f;
    [SerializeField] float percentageConsumptionInElementInBonusRound = 0.25f;
    [SerializeField] float lowerLimitOfContainers = 0.0001f;
    [SerializeField] float upperLimitOfContainers = 1f;
    //[SerializeField] float startingCapacityOfContainers = 0.1f;
    [SerializeField] Transform fireFluid;
    [SerializeField] Transform waterFluid;
    [SerializeField] Transform earthFluid;

    [SerializeField] TrailRenderer fireTerrainSpray;
    [SerializeField] TrailRenderer waterTerrainSpray;
    [SerializeField] TrailRenderer earthTerrainSpray;

    [SerializeField] Vector3 deltaTerrainSprayPosN;
    [SerializeField] Vector3 deltaTerrainSprayPosW;
    [SerializeField] Vector3 deltaTerrainSprayPosE;

    [SerializeField] GameObject fireSpray;
    [SerializeField] GameObject waterSpray;
    [SerializeField] GameObject earthSpray;

    [SerializeField] Color elementUiColor;
    [SerializeField] Color elementUiColor1;
    [SerializeField] Color elementUiColor2;
    

    [SerializeField] SpriteRenderer fireElementUi;
    [SerializeField] SpriteRenderer waterElementUi;
    [SerializeField] SpriteRenderer earthElementUi;

    [SerializeField] float frequecyOfColorChange = 0.2f;
    

    float[] elements = new float[3];     // 0 - fire
                                         // 1 - water
                                         // 2 - earth
                                         // 3 - ice

    PlayerController player;
   // float startingCapacityOfContainers;
   public GameSession gameSession;

    bool isFireTerrainSpray = false;
    bool isWaterTerrainSpray = false;
    bool isEarthTerrainSpray = false;

    bool isConsumeFire = false;
    bool isConsumeWater = false;
    bool isConsumeEarth = false;

    bool isBonus = false;

    Vector3 deltaTerrainSprayPos;

    AudioSource audioSource;

    int currTeerrainID;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        audioSource = GetComponent<AudioSource>();
        /*
        startingCapacityOfContainers = gameSession
        */

        ResetFuelTerrain();
    }

    private void ResetFuelTerrain()
    {
        gameSession = FindObjectOfType<GameSession>();
        for (int i = 0; i < 3; i++)
        {
            elements[i] = gameSession.lastElementsCapacity[i];
            Debug.Log(elements[i]);
        }

        //elements = gameSession.lastElementsCapacity;

        SetFire();
        SetWater();
        SetEarth();
        ResetAllTerrainSpray();

        deltaTerrainSprayPos = deltaTerrainSprayPosN;
    }


    // Update is called once per frame
    void Update()
    {
        /* North - 1
        * West - 2
        * East - 3 
        * South - 4*/
        if (gameSession == null)
            ResetFuelTerrain();
            //gameSession = FindObjectOfType<GameSession>();

        int dir = player.GetDir();
        switch (dir)
        {
            case 1:
                deltaTerrainSprayPos = deltaTerrainSprayPosN;
                break;

            case 2:
                deltaTerrainSprayPos = deltaTerrainSprayPosW;
                break;

            case 3:
                deltaTerrainSprayPos = deltaTerrainSprayPosE;
                break;

        }

        if (isFireTerrainSpray)
        {
            
            fireTerrainSpray.gameObject.transform.position = deltaTerrainSprayPos +
                transform.position;
            //fireTerrainSpray.gameObject.transform.rotation = transform.rotation;
        }

        if(isWaterTerrainSpray)
        {
            waterTerrainSpray.gameObject.transform.position = deltaTerrainSprayPos +
                transform.position;
            //waterTerrainSpray.gameObject.transform.rotation = transform.rotation;
        }

        if(isEarthTerrainSpray)
        {
            earthTerrainSpray.gameObject.transform.position = deltaTerrainSprayPos +
                transform.position;
            //earthTerrainSpray.gameObject.transform.rotation = transform.rotation;
        }

        if(isConsumeFire)
        {
            elements[0] = elements[0] - percentageConsumptionInElement;

            if (elements[0] < lowerLimitOfContainers)
            {
                elements[0] = lowerLimitOfContainers;
                //Debug.Log("empty container");
                //player.KillPlayer();
                ResetAllSpray();
                StartCoroutine(gameSession.Kill(false, currTeerrainID));
                isConsumeFire = false;
            }

            SetFire();
        }

        if(isConsumeWater)
        {
            elements[1] = elements[1] - percentageConsumptionInElement;

            if (elements[1] < lowerLimitOfContainers)
            {
                elements[1] = lowerLimitOfContainers;
                //Debug.Log("empty container");
                //player.KillPlayer();
                ResetAllSpray();
                StartCoroutine(gameSession.Kill(false, currTeerrainID));
                isConsumeWater = false;
            }

            SetWater();
        }

        if(isConsumeEarth)
        {
            elements[2] = elements[2] - percentageConsumptionInElement;

            if (elements[2] < lowerLimitOfContainers)
            {
                elements[2] = lowerLimitOfContainers;
                //Debug.Log("empty container");
                //player.KillPlayer();
                ResetAllSpray();
                StartCoroutine(gameSession.Kill(false, currTeerrainID));
                isConsumeEarth = false;
            }

            SetEarth();
        }

        if(isBonus)
        {
            if(elements[0] > lowerLimitOfContainers)
            {
                elements[0] = elements[0] - percentageConsumptionInElementInBonusRound;
                SetFire();
            }
            
            else if(elements[1] > lowerLimitOfContainers)
            {
                elements[1] = elements[1] - percentageConsumptionInElementInBonusRound;
                SetWater();
            }

            else if (elements[2] > lowerLimitOfContainers)
            {
                elements[2] = elements[2] - percentageConsumptionInElementInBonusRound;
                SetEarth();
            }

            else 
            {
                if (player.IsGrounded)
                {
                    isBonus = false;
                    gameSession.Win();
                }
            }
        }

    }

    public bool ConsumeFuel(int elementId, int currTeerrainID)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        // 3 - ice
        //Debug.Log("Before Decrement = " + elements[elementId]);
        // elements[elementId] = elements[elementId] - percentageConsumptionInElement;
        //  Debug.Log("Decrement = " + percentageConsumptionInElement);
        //Debug.Log("After Decrement = " + elements[elementId]);
       // this.currTeerrainID = currTeerrainID;
        audioSource.Play();
        this.currTeerrainID = currTeerrainID;
        //Debug.Log("consume");
        switch (elementId)
        {
            case 0:
                isFireTerrainSpray = true;
                isWaterTerrainSpray = false;
                isEarthTerrainSpray = false;
                isConsumeFire = true;
                //SetFire();
                //ActivateFireTerrainSpray(true);
                Spray(true, 0);


                break;

            case 1:
                isFireTerrainSpray = false;
                isWaterTerrainSpray = true;
                isEarthTerrainSpray = false;
                isConsumeWater = true;
               // SetWater();
               // ActivateWaterTerrainSpray(true);
                Spray(true, 1);

                break;

            case 2:
                isFireTerrainSpray = false;
                isWaterTerrainSpray = false;
                isEarthTerrainSpray = true;
                isConsumeEarth = true;
               // SetEarth();
               // ActivateEarthTerrainSpray(true);
                Spray(true, 2);

                break;
        }

        return true;
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
                StartCoroutine(BlinkFireUiElement(fireElementUi));
                SetFire();
                break;

            case 1:
                StartCoroutine(BlinkWaterUiElement(waterElementUi));
                SetWater();
                break;

            case 2:
                StartCoroutine(BlinkEarthUiElement(earthElementUi));
                SetEarth();
                break;
        }
    }


    private IEnumerator BlinkFireUiElement(SpriteRenderer sprite)
    {

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor1;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor1;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor;
    }

    private IEnumerator BlinkWaterUiElement(SpriteRenderer sprite)
    {

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor1;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor1;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor;
    }

    private IEnumerator BlinkEarthUiElement(SpriteRenderer sprite)
    {

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor1;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor1;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor2;

        yield return new WaitForSeconds(frequecyOfColorChange);

        sprite.color = elementUiColor;
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

    /*
    public bool ConsumeFuel(int elementId)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        //Debug.Log("Before Decrement = " + elements[elementId]);
        elements[elementId] = elements[elementId] - percentageConsumptionInElement;
      //  Debug.Log("Decrement = " + percentageConsumptionInElement);
       //Debug.Log("After Decrement = " + elements[elementId]);
        if (elements[elementId] < lowerLimitOfContainers)
        {
            elements[elementId] = lowerLimitOfContainers;
            //Debug.Log("empty container");
            //player.KillPlayer();
            StartCoroutine(gameSession.Kill());
            return false;
        }
        audioSource.Play();
        //Debug.Log("consume");
        switch (elementId)
        {
            case 0:
                isFireTerrainSpray = true;
                isWaterTerrainSpray = false;
                isEarthTerrainSpray = false;

                SetFire();
                ActivateFireTerrainSpray(true);
                Spray(true, 0);

                
                break;

            case 1:
                isFireTerrainSpray = false;
                isWaterTerrainSpray = true;
                isEarthTerrainSpray = false;

                SetWater();
                ActivateWaterTerrainSpray(true);
                Spray(true, 1);

                break;

            case 2:
                isFireTerrainSpray = false;
                isWaterTerrainSpray = false;
                isEarthTerrainSpray = true;

                SetEarth();
                ActivateEarthTerrainSpray(true);
                Spray(true, 2);

                break;
        }

        return true;
    }
    */

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
        isConsumeFire = false;
        isConsumeWater = false;
        isConsumeEarth = false;

        //ActivateFireTerrainSpray(false);
        //ActivateWaterTerrainSpray(false);
        //ActivateEarthTerrainSpray(false);

        ResetAllSpray();
    }


   

    public float[] GetElements()
    {
        return elements;
    }

    public void ElementalWallCheck(int elementalWallId,
        int counterElementNeededId,
        GameObject elementalWall)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        

        elements[counterElementNeededId] = elements[counterElementNeededId] - percentageConsumptionInElement;
        if (elements[counterElementNeededId] < lowerLimitOfContainers)
        {
            elements[counterElementNeededId] = lowerLimitOfContainers;
            
            StartCoroutine(gameSession.Kill(false, 4));
            return;
        }
        switch (elementalWallId)
        {
            case 0:   //need water - 1
                SetWater();
                break;

            case 1:  //need green - 2
                SetEarth();
                break;

            case 2:  //nee fire - 0
                SetFire();
                break;
        }

        elementalWall.GetComponent<ElementalWall>().Destroy();
        //Destroy(elementalWall);
    }

    public void ElementalEnemyCheck(int elementalEnemyId,
        int counterElementNeededId,
        GameObject elementalEnemy)
    {
        // 0 - fire
        // 1 - water
        // 2 - earth
        

        elements[counterElementNeededId] = elements[counterElementNeededId] - percentageConsumptionInElement;
        if (elements[counterElementNeededId] < lowerLimitOfContainers)
        {
            elements[counterElementNeededId] = lowerLimitOfContainers;

            StartCoroutine(gameSession.Kill(false, 4));
            return;
        }
        switch (elementalEnemyId)
        {
            case 0:   //need water - 1
                SetWater();
                break;

            case 1:  //need green - 2
                SetEarth();
                break;

            case 2:  //nee fire - 0
                SetFire();
                break;
        }

        elementalEnemy.GetComponent<ElementalEnemy>().Destroy();
        //Destroy(elementalWall);
    }

    void Spray(bool activate, int spray)
    {
        // // 0 - fire
        // 1 - water
        // 2 - earth

        switch(spray)
        {
            case 0:
                fireSpray.SetActive(activate);
                break;

            case 1:
                waterSpray.SetActive(activate);
                break;

            case 2:
                earthSpray.SetActive(activate);
                break;
        }
    }

    private void ResetAllSpray()
    {
        Spray(false, 0);
        Spray(false, 1);
        Spray(false, 2);
        audioSource.Stop();
    }

    public void SetBonus(bool val)
    {
        isBonus = val;
    }
}
