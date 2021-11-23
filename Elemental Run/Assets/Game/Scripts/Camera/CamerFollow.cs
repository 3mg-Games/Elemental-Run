using System.Collections;
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

    /*[SerializeField] Vector3 deltaN = new Vector3(4.2f, -2.9f, 0f);
    [SerializeField] Vector3 deltaW = new Vector3(4.2f, -2.9f, 0f);
    [SerializeField] Vector3 deltaE = new Vector3(4.2f, -2.9f, 0f);
    [SerializeField] Vector3 deltaS = new Vector3(4.2f, -2.9f, 0f);

    [SerializeField] int dir = 1;*/
    float camZ;
    Vector3 delta;

    bool isWin = false;

    private Vector3 point;
    // Start is called before the first frame update
    void Start()
    {
        /*
        delta.x = player.position.x - transform.position.x;
        delta.y = player.position.y - transform.position.y;
        delta.z = player.position.z - transform.position.z;*/
        // Debug.Log(delta);
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

        //  delta = deltaN;
    }

    private void LateUpdate()
    {
        if (!isWin)
        {
            //transform.position = new Vector3(player.position.x - delta.x, player.position.y - delta.y, player.position.z - delta.z);
        }
        if (isWin)
        {
            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedMod);
            //transform.LookAt(player.transform);
            // transform.Translate(-Vector3.right * rotateSpeed * Time.deltaTime);
        }
    }

    public void ActivateRotate()
    {
        transform.parent = null;
        point = player.transform.position;
        transform.LookAt(point);
        isWin = true;
    }

    // Update is called once per frame

}
