using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Panel : MonoBehaviour
{
    public GameObject actionPanel;
    public GameObject addCommentPanel;
    // Start is called before the first frame update
    void Start()
    {
        if (actionPanel && addCommentPanel)
        {
            actionPanel.SetActive(true);
            addCommentPanel.SetActive(false);
        }
    }

    public void SwitchPanel()
    {
        if (actionPanel.activeSelf)
        {
            actionPanel.SetActive(false);
            addCommentPanel.SetActive(true);
        }
        else
        {
            actionPanel.SetActive(true);
            addCommentPanel.SetActive(false);
        }
    }
}
