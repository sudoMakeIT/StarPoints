using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    public GameObject button1;
    public GameObject button2;
    // Start is called before the first frame update
    void Start()
    {
        if (button1 && button2)
        {
            button1.SetActive(true);
            button2.SetActive(false);
        }
    }

    public void SwitchButton()
    {
        if (button1.activeSelf)
        {
            button1.SetActive(false);
            button2.SetActive(true);
        }
        else
        {
            button1.SetActive(true);
            button2.SetActive(false);
        }
    }
}
