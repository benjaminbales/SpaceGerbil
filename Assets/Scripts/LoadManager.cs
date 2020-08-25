
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadManager : MonoBehaviour
{ 
    public string NextLevel;
    public float timerToLoadNextLevel = 5f;
    public bool LoadLevelAfterTimer = true;
    // Start is called before the first frame update
    void Start()
    {
        // if we want to tell teh game to load the level after n seconds, then begin that timer
        if( LoadLevelAfterTimer )
            Invoke("TimerLoadScene", timerToLoadNextLevel);
        // else the script will await for further instruction
    }

    // this is the script that gets called after time interval has passed- it's private for a reason
    private void TimerLoadScene()
    {
        LoadScene(NextLevel);
    }

    public void LoadSceneWithTimer(string name = null)
    {
        NextLevel = name;
        Invoke("LoadScene", timerToLoadNextLevel);
    }

    // public available method to access and load scene from other inputs
    public void LoadScene(string name = null)
    {
        // by default name is null unless override by other inputs, we'll use the NextLevel instead. 
        SceneManager.LoadScene(name ?? NextLevel);
    }


    public void LoadScene()
    {
        // by default name is null unless override by other inputs, we'll use the NextLevel instead. 
        SceneManager.LoadScene(NextLevel);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
