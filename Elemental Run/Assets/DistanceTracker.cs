using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    [Tooltip("North - 1, West - 2, East - 3, South - 4")]
    [SerializeField] int dir;
    ProgressBar progresBar;

    // Start is called before the first frame update
    void Start()
    {
        progresBar = FindObjectOfType<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            progresBar.ChangeDir(dir, transform.position);
        }
    }
}
