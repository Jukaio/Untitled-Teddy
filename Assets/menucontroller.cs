using System.Collections;
using System.Collections.Generic;
using UnityEngine;
UnityEngine.SceneManagement;
public class menucontroller : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame()
    {
        SceneManager.LoadScene("OtherSceneName", LoadSceneMode.Additive);
    }

}
