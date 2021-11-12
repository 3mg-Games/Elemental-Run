using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalEntry : MonoBehaviour
{
    // 0 - fire
    // 1 - water
    // 2 - earth
    [SerializeField] int elementId = 0;


    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameSession.ActivateElementSelectionPanel(elementId);
        }
    }
}
