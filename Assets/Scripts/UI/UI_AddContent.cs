using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UI_AddContent : MonoBehaviour
{
    public GameObject panelOriginal;
    public GameObject container;
    public GameObject scrollbar;

    void Start()
    {
        CreatePanel(10);
    }

    private void CreatePanel(int panelNum)
    {
        for (int i = 1; i < panelNum; i++)
        {
            GameObject panelClone = Instantiate(panelOriginal);
            panelClone.name = "Level (" + (i + 1) + ")";
            panelClone.transform.parent = container.transform;
        }
    }

    // public void removePanel()
    // {
    //     Debug.Log(Math.Round(10 - (container.GetComponent<RectTransform>().sizeDelta.x + container.transform.position.x)));
    // }

    public void removePanel()
    {
        float scroll_pos = 0;
        float[] pos;

        pos = new float[transform.childCount];
        float dist = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = dist * i;
        }

        scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (dist / 2) && scroll_pos > pos[i] - (dist / 2))
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }


}
