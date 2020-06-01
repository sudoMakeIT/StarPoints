using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text : MonoBehaviour
{
    [TextArea]
    public string MyTextArea;
    public void ChangeText(Text aText, string str)
    {
        aText.text = str;
    }

}
