﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] float maxDistance;
    //[SerializeField] 

    PlayerController player;
    Image progressBar;
    float playerDistance;
    Vector3 prevPlayerPos;
    public int playerDir = 1;
    float basePlayerDist;
    float lastDistance;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        progressBar = GetComponent<Image>();

        //assuming player always starts from north
        prevPlayerPos = player.transform.position;
        basePlayerDist = playerDistance = prevPlayerPos.x;
        progressBar.fillAmount = playerDistance / maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(progressBar.fillAmount < 1)
        {
            // int playerDir = player.GetDir();
            var diff = Vector3.zero;
            switch (playerDir)
            {
                case 1: //north
                    diff = player.transform.position - prevPlayerPos;
                    playerDistance = basePlayerDist + diff.x;
                    progressBar.fillAmount = playerDistance / maxDistance;
                    break;

                case 2: //west
                    diff = player.transform.position - prevPlayerPos;
                    playerDistance = basePlayerDist + diff.z;
                    progressBar.fillAmount = playerDistance / maxDistance;
                    break;

                case 3: //east
                    diff = player.transform.position  - prevPlayerPos;
                    //float var = diff.z;
                    

                    playerDistance = basePlayerDist + diff.z * -1;
                    progressBar.fillAmount = playerDistance / maxDistance;
                    break;
            }
            
        }
    }


    public void ChangeDir(int dir, Vector3 pos)
    {
        switch(dir)
        {
            case 1:
                basePlayerDist = playerDistance;
                prevPlayerPos = pos;
                playerDir = dir;
                break;

            case 2:
                basePlayerDist = playerDistance;
                prevPlayerPos = pos;
                playerDir = dir;
                break;


            case 3:
                basePlayerDist = playerDistance;
                prevPlayerPos = pos;
                playerDir = dir;
                break;
        }
    }
}
