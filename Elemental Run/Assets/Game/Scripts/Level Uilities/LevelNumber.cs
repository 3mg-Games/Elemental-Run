using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script for maintaing the correct level number
public class LevelNumber : MonoBehaviour
{
    [SerializeField] int levelNumber = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public void SetLevelNumber(int value)
    {
        levelNumber = value;
    }
}
