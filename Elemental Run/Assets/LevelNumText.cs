using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNumText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumText;
    // Start is called before the first frame update
    void Start()
    {
        levelNumText.text = FindObjectOfType<LevelNumber>().GetLevelNumber().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
