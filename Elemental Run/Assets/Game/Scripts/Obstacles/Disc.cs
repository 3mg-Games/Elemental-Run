using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disc : MonoBehaviour
{
    [SerializeField] GameObject path;
    [SerializeField] float movementSpeed = 5f;

    GameSession gameSession;

    int wayPointsIndex;
    int wayPointsCount;

    List<Transform> waypoints = new List<Transform>();

    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        wayPointsIndex = 0;
        wayPointsCount = path.transform.childCount;
        for(int i = 0; i < wayPointsCount; i++)
        {
            waypoints.Add(path.transform.GetChild(i));
        }

        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession == null)
            gameSession = FindObjectOfType<GameSession>();
        if (wayPointsIndex <= wayPointsCount - 1)
        {
            Vector3 dir;

            //verticalInput = 1;
            //transform.forward = new Vector3(-verticalInput, 0, Mathf.Abs(verticalInput) - 1);

            var targetPosition = waypoints[wayPointsIndex].transform.position;

            var movementThisFrame = movementSpeed * Time.deltaTime;
            //var rotationThisFrame = turnRotateSpeed * Time.deltaTime;
            dir = targetPosition - transform.position;
           // Quaternion rotation = Quaternion.LookRotation(-dir, Vector3.up);
            //Debug.Log(rotation);

            transform.position = Vector3.MoveTowards        //try character controller here
                      (transform.position, targetPosition, movementThisFrame);
            //transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationThisFrame);


            if (transform.position == targetPosition)
                wayPointsIndex++;


        }

        else
        {
            Transform temp;
            temp = waypoints[0];
            waypoints[0] = waypoints[1];
            waypoints[1] = temp;
            wayPointsIndex = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(gameSession.Kill(false));

        }
    }
}
