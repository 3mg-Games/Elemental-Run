using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    float camZ;
    [SerializeField] Vector3 delta = new Vector3(4.2f, -2.9f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        /*
        delta.x = player.position.x - transform.position.x;
        delta.y = player.position.y - transform.position.y;
        delta.z = player.position.z - transform.position.z;*/
        Debug.Log(delta);

    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x - delta.x, transform.position.y, player.position.z - delta.z);
    }

    // Update is called once per frame

}
