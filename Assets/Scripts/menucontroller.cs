using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menucontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
	
void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenu();
        }
}

public void LoadMenu()
    {
        SceneManager.LoadScene("menu", LoadSceneMode.Single);
    }
}
