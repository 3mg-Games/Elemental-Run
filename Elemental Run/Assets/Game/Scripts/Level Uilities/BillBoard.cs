using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for billboarding a gameobject
//i.e., the gameobect that has this script will always face camera
public class BillBoard : MonoBehaviour
{
    [SerializeField] Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
