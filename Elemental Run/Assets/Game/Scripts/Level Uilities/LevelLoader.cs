using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        int idx = SceneManager.GetActiveScene().buildIndex + 1;
        if (idx >= SceneManager.sceneCountInBuildSettings)
            idx = 0;
        LoadScene(idx);
    }

    private void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetCurrentSceneBuildIdx()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
