using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectElement : MonoBehaviour
{
    
    public GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        if(gameSession == null)
        {
            Debug.Log("game session missing");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSession == null)
            gameSession = FindObjectOfType<GameSession>();
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
