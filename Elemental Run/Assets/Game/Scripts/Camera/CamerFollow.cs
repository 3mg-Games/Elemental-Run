﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    /* North - 1
      * West - 2
      * East - 3 
      * South - 4*/
    [SerializeField] Transform player;
    [SerializeField] float speedMod = 10f;

    [SerializeField] Vector3 lWallRunEulerAngles;
    [SerializeField] Vector3 rWallRunEulerAngles;

    [SerializeField] GameObject cam2;
    [SerializeField] float currentAngleDelta;

    [SerializeField] bool isPlayerFollow = false;

    [SerializeField] Vector3 deltaN = new Vector3(4.2f, -2.9f, 0f);
    /*[SerializeField] Vector3 deltaW = new Vector3(4.2f, -2.9f, 0f);
    [SerializeField] Vector3 deltaE = new Vector3(4.2f, -2.9f, 0f);
    [SerializeField] Vector3 deltaS = new Vector3(4.2f, -2.9f, 0f);

    [SerializeField] int dir = 1;*/
    float camZ;
    Vector3 delta;

    bool isWin = false;

    private Vector3 point;

    private Vector3 orignialRotEulerAngles;

    bool isCamRotate = false;
    Quaternion curRotation;
    Quaternion targetRotation;

    //PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
       // player
        /*
        delta.x = player.position.x - transform.position.x;
        delta.y = player.position.y - transform.position.y;
        delta.z = player.position.z - transform.position.z;
         Debug.Log(delta);*/
        /*
        switch (dir)
        {
            case 1:
                delta = deltaN;
                break;

            case 2:
                delta = deltaW;
                Debug.Log("is west");
                break;

            case 3:
                delta = deltaE;
                break;

            case 4:
                delta = deltaS;
                break;
        }*/

        delta = deltaN;
        orignialRotEulerAngles = transform.rotation.eulerAngles;
    }

  

    private void LateUpdate()
    {
        if (!isWin && isPlayerFollow)
        {
            transform.position = new Vector3(player.position.x - delta.x, player.position.y - delta.y, player.position.z - delta.z);
        }
        if (isWin)
        {
            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedMod);
            //transform.LookAt(player.transform);
            // transform.Translate(-Vector3.right * rotateSpeed * Time.deltaTime);
        }

        if(isCamRotate)
        {
            WallRunCamRoate();
        }
    }

    private void WallRunCamRoate()
    {
        /*curRotation = cam2.transform.rotation;
        //= transform.rotation;
        //if (smooth)
        // {
        // currentAngleDelta = SmoothDelay(currentAngleDelta, angle, smoothDelay);
        //}
        //else
        //{

        //}
        curRotation *= Quaternion.Euler(0f, 0f, currentAngleDelta);
        Debug.Log("Curr rot z = " + curRotation.eulerAngles.z + " Target Rot z = " + targetRotation.eulerAngles.z);
        
        if (curRotation.eulerAngles.z >= targetRotation.eulerAngles.z)
        {
            isCamRotate = false;
            return;
        }
        cam2.transform.rotation = curRotation;*/
    }


    public void ActivateRotate()
    {
        SetParentNull();
        point = player.transform.position;
        transform.LookAt(point);
        isWin = true;
    }

    public void SetParentNull()
    {
        transform.parent = null;
    }

    public void SwitchToWallRunCam(int dir)
    {
        /* 1 - left
         * 2 - Right
         * */

        if (dir == 1)
        // transform.rotation = Quaternion.Euler(lWallRunEulerAngles); 
        {
            cam2.SetActive(true);
            targetRotation = Quaternion.Euler(lWallRunEulerAngles);
            isCamRotate = true;
        }


        else
        {
            //transform.rotation = Quaternion.Euler(rWallRunEulerAngles);
            cam2.SetActive(true);
            targetRotation = Quaternion.Euler(rWallRunEulerAngles);
            isCamRotate = true;
        }
            

    }

    public void SwitchToNormalCam()
    {
        //transform.rotation = Quaternion.Euler(orignialRotEulerAngles);
        cam2.SetActive(false);
    }

    
    // Update is called once per frame

}
