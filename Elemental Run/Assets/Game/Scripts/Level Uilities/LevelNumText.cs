using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//script for maintaing level number text
public class LevelNumText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumText;

    LevelNumber levelNumber;
    // Start is called before the first frame update
    void Start()
    {
        levelNumber = FindObjectOfType<LevelNumber>();
        levelNumText.text = levelNumber.GetLevelNumber().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        levelNumText.text = levelNumber.GetLevelNumber().ToString();
    }
}
