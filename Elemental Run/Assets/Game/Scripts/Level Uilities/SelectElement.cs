using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectElement : MonoBehaviour
{
    
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

    public void SetID(int id)
    {
        gameSession.DeactivateElementSelectionPanel(id);
    }

    public void Continue()
    {
        gameSession.Continue();
    }


}
