using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Left : MonoBehaviour
{
    public Sprite sprite;
    public GameObject img;
    public GameObject txt;
    public Button button;

    public void Start()
    {
        button.onClick.AddListener(delegate { ChangeImg(); });
    }

    private void ChangeImg()
    {
        img.GetComponent<Image>().sprite = sprite;
        txt.GetComponent<TextMeshProUGUI>().text = "Sagittarius";

    }

}
