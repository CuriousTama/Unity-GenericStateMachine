using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPrintedElement : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;

    public void SetString(string text)
    {
        this.text.text = text;
    }
}
