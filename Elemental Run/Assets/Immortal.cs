using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameAnalyticsSDK;
using Facebook.Unity;

public class Immortal : MonoBehaviour
{
    private static Immortal instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);


        }


        else
        {
            Destroy(this.gameObject);
        }

        FB.Init();
    }
    private void Start()
    {
        GameAnalytics.Initialize();
    }

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LevelComplete(int levelNum)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, levelNum.ToString("D4"));
        print("Level Complete");
    }

    public void LevelFail(int levelNum)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, levelNum.ToString("D4"));
        print("Level Fail");
    }
}
