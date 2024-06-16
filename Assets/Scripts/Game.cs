using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public UI UI;
    public GameObject choicePanel;
    
    public void OnStartButtonClicked()
    {
        UI.HideStartScreen();
    }
    
    public ButtonTextAssigner buttonTextAssigner; // Reference to your button text assigner script

    void Start()
    {
        string result = StoryNavigator.ValidateStoryPoints("StoryDataReal.csv");
        print(result);
        StoryNavigator.LoadStoryPoints("StoryDataReal.csv");
        
        // Set the choice panel reference in StoryNavigator
        StoryNavigator.SetChoicePanel(choicePanel);
        
        StoryNavigator.HideChoicePanel();
        
        // Set the button text assigner reference in StoryNavigator
        StoryNavigator.SetButtonTextAssigner(buttonTextAssigner);
        
        // Display choices on button


    }

    public void Restart()
    {
        StoryNavigator.Reset();
        SceneManager.LoadScene("Menu");
    }
}
