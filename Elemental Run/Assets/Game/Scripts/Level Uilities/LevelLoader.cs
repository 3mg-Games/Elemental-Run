using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    private void Awake()
    {
        //int startScene = PlayerPrefs.GetInt("Level", 0);
        //LoadScene(startScene);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void LoadNextScene(bool isLevel10Finished)
    {

        if (!isLevel10Finished)
        {
            //if level 10 isnt finished, load next scene from build indices
            int idx = SceneManager.GetActiveScene().buildIndex + 1;
            if (idx >= SceneManager.sceneCountInBuildSettings)
            {
                idx = 0;
            }
            LoadScene(idx);
        }

        else
        {
            //if level 10 is finished, then load random level beween 3 to 10
            int randomLevel = UnityEngine.Random.Range(3, 11);
           
            PlayerPrefs.SetInt("Level", randomLevel);
            /*int idx = SceneManager.GetActiveScene().buildIndex + 1;
            if (idx >= SceneManager.sceneCountInBuildSettings)
            {
                idx = 2;
            }*/
            Debug.Log("Random Level = " + randomLevel);
            Debug.Log("Player Prefs val = " + PlayerPrefs.GetInt("Level"));
            LoadScene(randomLevel - 1);
        }
    }

    private void LoadScene(int index)
    {
       //PlayerPrefs.SetInt("Level", index+1);
        SceneManager.LoadScene(index);
    }

    public void LoadCurrentScene()
    {
        //load current scene again
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCurrentSceneBuildIdx()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadParticularScene(int idx)
    {
        LoadScene(idx);
    }

    public int GetTotalSceneCount()
    {
        return SceneManager.sceneCountInBuildSettings;
    }
}
