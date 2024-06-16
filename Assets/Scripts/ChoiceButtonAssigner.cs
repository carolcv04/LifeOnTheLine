using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ButtonTextAssigner : MonoBehaviour
{
    public Button[] buttons; // Array to store the buttons

    public void AssignButtonText()
    {
        StoryPoint storyPoint = StoryNavigator.GetCurrentStoryPoint();
        List<string> choices = StoryNavigator.GetCurrentChoices(); // Call your method to get the list of choices
        Debug.Log("Printing text");
        Debug.Log(choices.Count);
        // Assign text to each button
        for (int i = 0; i < buttons.Length; i++)
        {
            
            if (i < choices.Count)
            {
                buttons[i].GetComponentInChildren<TMP_Text>().text = choices[i];
            }
            else
            {
                // Disable the button if there are no more choices
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}