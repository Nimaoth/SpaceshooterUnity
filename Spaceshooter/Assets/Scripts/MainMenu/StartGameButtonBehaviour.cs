using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButtonBehaviour : MonoBehaviour {

    public void LoadLevelByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadLevelByName("Level1");
        }
    }
}
