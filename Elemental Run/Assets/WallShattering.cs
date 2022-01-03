using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for shaterring the wall of wall run
public class WallShattering : MonoBehaviour
{
    Rigidbody rb;
    Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
           // Debug.Log("Wall trigger");
           /*
            collider.isTrigger = false;
            rb.isKinematic = false;
            var force = Random.Range(5f, 8f);
            rb.AddForce(force * Vector3.right);
            Destroy(gameObject, 1.3f);*/
        }
    }
}
