using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour
{
    [SerializeField] float coinMoveSpeed = 10f;
    GameSession gameSession;
    //RectTransform target;

    RectTransform coinRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
       // target = gameSession.GetCoinSpawnTarget();
        coinRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if ( <= turnWayPointsCount - 1)
        
        Vector3 dir;

        //var targetPosition = target.position;
        var targetPosition = gameSession.GetCoinSpawnTarget().position;
        var movementThisFrame = coinMoveSpeed * Time.deltaTime;

        //Debug.Log(targetPosition);
        dir = targetPosition - coinRectTransform.position;
         
        transform.position = Vector3.MoveTowards        //try character controller here
                      (transform.position, targetPosition, movementThisFrame);
              if (transform.position == targetPosition) 
           Destroy();


    }

    private void Destroy()
    {
        gameSession.RealCoinInc();
        Destroy(gameObject);
    }
}

