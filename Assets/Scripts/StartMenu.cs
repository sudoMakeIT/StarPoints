using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public void StartApp()
    {
        SceneManager.LoadScene(1);
    }

    public void Login()
    {
        SceneManager.LoadScene(2);
    }

    public void Next()
    {
        SceneManager.LoadScene(3);
    }

}
