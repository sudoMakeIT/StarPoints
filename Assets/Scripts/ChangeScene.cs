using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeString(string aScene)
    {
        SceneManager.LoadScene(aScene);
    }

    public void ChangeInt(int aScene)
    {
        SceneManager.LoadScene(aScene);
    }
}
