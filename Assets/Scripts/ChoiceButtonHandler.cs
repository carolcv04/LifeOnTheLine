using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using Button = UnityEngine.UI.Button;

public class ChoiceButtonHandler : MonoBehaviour
{
    public ButtonTextAssigner ButtonTextAssigner;
    public Mover mover;
        
    public void OnChoiceButtonClick()
    {
        // Get the clicked GameObject
        GameObject clickedObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        if (clickedObject != null)
        {
            // Get the Button component from the clicked GameObject
            Button clickedButton = clickedObject.GetComponent<Button>();

            if (clickedButton != null)
            {
                // Get the text component of the clicked button
                TMP_Text buttonText = clickedButton.GetComponentInChildren<TMP_Text>();

                if (buttonText != null)
                {
                    // Get the text of the button
                    string choiceText = buttonText.text;

                    // Call GetNextStoryPoint with the clicked choice text
                    //StoryNavigator.GetNextStoryPoint(choiceText);
                    mover.MoveNextChoice(choiceText);
                }
                else
                {
                    Debug.LogWarning("Button text component not found!");
                }
            }
            else
            {
                Debug.LogWarning("Button component not found!");
            }
        }
        else
        {
            Debug.LogWarning("Clicked object not found!");
        }
    }
}