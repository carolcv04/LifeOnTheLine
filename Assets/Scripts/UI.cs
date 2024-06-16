using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public CanvasGroup StartScreenCanvasGroup;
    public void HideStartScreen()
    {
        CanvasGroupDisplayer.Hide(StartScreenCanvasGroup);
    }
}