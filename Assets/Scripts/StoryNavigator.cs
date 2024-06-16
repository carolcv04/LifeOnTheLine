using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StoryNavigator
{
    private static Dictionary<string, StoryPoint> storyPoints = new Dictionary<string, StoryPoint>();
    private static StoryPoint currentStoryPoint = null;

    private static String currentBackgroundSpriteName;
    private static String currentStoryPointUniqueName = "Start";
    
    private static GameObject choicePanel;
    private static ButtonTextAssigner buttonTextAssigner;

    public static void Reset()
    {
        storyPoints = new Dictionary<string, StoryPoint>();
    }
    
    // Set reference to the button text assigner
    public static void SetButtonTextAssigner(ButtonTextAssigner assigner)
    {
        buttonTextAssigner = assigner;
    }

    // Method to display choices on buttons
    public static void DisplayChoicesOnButtons()
    {
        if (currentStoryPoint.Type == StoryPointType.Choice || currentStoryPoint.Type == StoryPointType.RChoice)
        {
            if (buttonTextAssigner != null)
            {
                buttonTextAssigner.AssignButtonText();
                Debug.Log("hello !");
                ShowChoicePanel();
            }
        }
        else
        {
            HideChoicePanel();
        }
    }

    // Method to handle button click
    public static void OnChoiceButtonClick(string choiceText)
    {
        GetNextStoryPoint(choiceText);
    }

    // Set reference to the choice panel
    public static void SetChoicePanel(GameObject panel)
    {
        choicePanel = panel;
    }

    // Hide the choice panel
    public static void HideChoicePanel()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(false);
        }
    }

    // Show the choice panel
    private static void ShowChoicePanel()
    {
        if (choicePanel != null)
        {
            choicePanel.SetActive(true);
        }
    }
    public static string ValidateStoryPoints(string filePath)
    {
        int lineNumber = 0;
        List<string> errors = new List<string>();
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    string[] parts = line.Split(',');
                    if (parts.Length != 7)
                    {
                        errors.Add($"Line {lineNumber}: Incorrect number of fields ({parts.Length}). Expected 7 fields.");
                        continue;
                    }

                    string storyPointType = parts[0].Trim('"');
                    string uniqueNameIDForStoryPoint = parts[1].Trim('"');
                    string textToBeShownToPlayer = parts[2].Trim('"');
                    string textToBeShownForChoice = parts[3].Trim('"');
                    string destinationStoryPoint = parts[4].Trim('"');
                    string spriteName = parts[5].Trim('"');
                    string numberOfChoicesShown = parts[6].Trim('"');
                    
                    if (!Enum.TryParse(storyPointType, out StoryPointType type))
                    {
                        errors.Add($"Line {lineNumber}: Invalid story point type '{storyPointType}'.");
                    }
                    
                    if (type == StoryPointType.RChoice && string.IsNullOrEmpty(numberOfChoicesShown))
                    {
                        errors.Add($"Line {lineNumber}: 'NumberOfChoicesShown' is required for 'RandomChoice' types.");
                    }
                    
                    if (string.IsNullOrEmpty(uniqueNameIDForStoryPoint) || string.IsNullOrEmpty(textToBeShownToPlayer))
                    {
                        errors.Add($"Line {lineNumber}: Essential fields cannot be empty.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return $"Failed to read file: {ex.Message}";
        }

        if (errors.Count == 0)
            return "No errors found. File is valid.";

        return string.Join("\n", errors);
    }

    public static void LoadStoryPoints(string filePath)
    {
        if (storyPoints.Count > 0)
            return;
        
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] fields = line.Split(',');
                if (fields.Length < 6) continue;

                StoryPointType type = Enum.TryParse(fields[0], true, out StoryPointType parsedType)
                    ? parsedType
                    : StoryPointType.Undefined;
                string uniqueName = fields[1].Trim('"');
                string text = fields[2].Trim('"');
                string choiceText = fields[3].Trim('"');
                string destination = fields[4].Trim('"');
                string spriteName = fields[5].Trim('"');
                int randomCount = type == StoryPointType.RChoice ? int.Parse(fields[6].Trim('"')) : 0;

                if (!storyPoints.ContainsKey(uniqueName))
                {
                    storyPoints[uniqueName] = new StoryPoint(type, uniqueName, text, spriteName, randomCount);
                }

                if (type == StoryPointType.REvent)
                {
                    storyPoints[uniqueName].Choices.Add(new Choice
                        { TextToShow = text, DestinationStoryPoint = destination });
                }
                else
                {
                    storyPoints[uniqueName].Choices.Add(new Choice
                        { TextToShow = choiceText, DestinationStoryPoint = destination });
                }

            }
        }

        PrintStoryPoints();
        
        MakeRandomSelections();

        FillInEmptyChoiceText();

        PrintStoryPoints("FinalStoryPoints.txt");
        currentStoryPoint = storyPoints["Start"];
    }

    private static void MakeRandomSelections()
    {
        foreach (KeyValuePair<string,StoryPoint> keyValuePair in storyPoints)
        {
            if (storyPoints[keyValuePair.Key].Type == StoryPointType.REvent)
            {
                StoryPoint sp = storyPoints[keyValuePair.Key];
                int randomEventIndex = Random.Range(0, storyPoints[keyValuePair.Key].Choices.Count);
                Choice selectedEvent = storyPoints[keyValuePair.Key].Choices[randomEventIndex];
                storyPoints[keyValuePair.Key].Type = StoryPointType.Event;
                storyPoints[keyValuePair.Key].Text = selectedEvent.TextToShow;
                storyPoints[keyValuePair.Key].Choices = new List<Choice>();
                storyPoints[keyValuePair.Key].Choices.Add(selectedEvent);
            }
            if (storyPoints[keyValuePair.Key].Type == StoryPointType.RChoice)
            {
                List<Choice> selectedChoices = new List<Choice>();
                int numberOfChoicesToSelect = storyPoints[keyValuePair.Key].RandomCount;
                for (int i = 0; i < numberOfChoicesToSelect; i++)
                {
                    int randomEventIndex = Random.Range(0, storyPoints[keyValuePair.Key].Choices.Count);
                    selectedChoices.Add(storyPoints[keyValuePair.Key].Choices[randomEventIndex]);
                    storyPoints[keyValuePair.Key].Choices.RemoveAt(randomEventIndex);
                }
                storyPoints[keyValuePair.Key].Choices = new List<Choice>();
                foreach (Choice selectedChoice in selectedChoices)
                {
                    storyPoints[keyValuePair.Key].Choices.Add(selectedChoice);
                }
                storyPoints[keyValuePair.Key].Type = StoryPointType.Choice;
            }
        }
    }

    private static void FillInEmptyChoiceText()
    {
        foreach (KeyValuePair<string, StoryPoint> keyValuePair in storyPoints)
        {
            if (storyPoints[keyValuePair.Key].Choices.Count == 1)
            {
                if (storyPoints[keyValuePair.Key].Choices[0].TextToShow == "")
                    storyPoints[keyValuePair.Key].Choices[0].TextToShow = "OK";
            }
        }
    }

    public static string GetNextBackgroundSpriteName()
    {
        if (currentStoryPoint.BackgroundSpriteName == String.Empty)
            return currentBackgroundSpriteName;
        else
        {
            currentBackgroundSpriteName = currentStoryPoint.BackgroundSpriteName;
            return currentBackgroundSpriteName;
        }
    }

    public static string GetCurrentBackgroundSpriteName()
    {
        return currentBackgroundSpriteName;
    }
    public static string GetCurrentMessage()
    {
        return currentStoryPoint.GetTextMessage();
    }

    public static List<String> GetCurrentChoices()
    {
        List<string> results = new List<string>();
        Debug.Log("CHOICES NUM: " + currentStoryPoint.Choices.Count);

        foreach (Choice choice in currentStoryPoint.Choices)
        {
            results.Add(choice.TextToShow);
        }
        
        return results;
    }

    public static StoryPoint GetNextStoryPoint()
    {
        return GetNextStoryPoint(null);
    }
    
    public static StoryPoint GetCurrentStoryPoint()
    {
        return currentStoryPoint;
    }

    public static StoryPoint GetStoryPoint(string uniqueName)
    {
        return storyPoints[uniqueName];
    }
    
    public static StoryPoint GetNextStoryPoint(string choiceText = null)
    {
        HideChoicePanel(); // Hide the panel when navigating to the next story point
        DebugLogger.Log("Getting next story point");
        DebugLogger.Log("    Current story point: " + currentStoryPointUniqueName);
        DebugLogger.Log("    Current type: " + currentStoryPoint.Type);
        DebugLogger.Log("    Current message: " + currentStoryPoint.GetTextMessage());
        DebugLogger.Log("    Choice: " + choiceText);
        
        StoryPoint nextStoryPoint = GetNextStoryPoint(currentStoryPointUniqueName, choiceText);
        DebugLogger.Log("    Next story point: " + nextStoryPoint.UniqueName);
        DebugLogger.Log("    Next type: " + nextStoryPoint.Type);
        DebugLogger.Log("    Next story message: " + nextStoryPoint.GetTextMessage());
        
        //ShowChoicePanel();
        return nextStoryPoint;
    }

    private static StoryPoint GetNextStoryPoint(string uniqueName, string choiceText)
    {
        MonoBehaviour.print("Getting next story point after: " + uniqueName);
        if (!storyPoints.ContainsKey(uniqueName))
        {
            return null;
        }

        Debug.Log("UNIQUE NAME: " + uniqueName);
        Debug.Log("CHOICE NAME: " + choiceText);
        StoryPoint storyPoint = FindNextStoryPointUniqueName(uniqueName, choiceText);
        MonoBehaviour.print("  Next story point is: " + storyPoint.UniqueName);

        // switch (storyPoint.Type)
        // {
        //     case StoryPointType.REvent:
        //         int randomEventIndex = Random.Range(0, storyPoint.Choices.Count);
        //         Choice selectedEvent = storyPoint.Choices[randomEventIndex];
        //         return storyPoints[selectedEvent.DestinationStoryPoint];
        //         
        //     case StoryPointType.RChoice:
        //         // Randomly select one of the choices
        //         int randomIndex = Random.Range(0, storyPoint.Choices.Count);
        //         Choice selectedChoice = storyPoint.Choices[randomIndex];
        //         // Proceed to the destination of the selected choice
        //         return storyPoints[selectedChoice.DestinationStoryPoint];
        // }

        currentStoryPoint = storyPoint;
        currentStoryPointUniqueName = storyPoint.UniqueName;

        return storyPoint;
    }
    
    public static Boolean IsGameOver()
    {
        if (currentStoryPoint.Type == StoryPointType.Ending)
            return true;
        return false;
    }
    
    public static List<Choice> GetRandomElements(List<Choice> choices, int elementCount)
    {
        elementCount = Math.Min(elementCount, choices.Count);
        List<Choice> shuffledChoices = new List<Choice>(choices);
        
        System.Random rng = new System.Random();
        for (int i = shuffledChoices.Count - 1; i > 0; i--)
        {
            int swapIndex = rng.Next(i + 1);
            Choice temp = shuffledChoices[i];
            shuffledChoices[i] = shuffledChoices[swapIndex];
            shuffledChoices[swapIndex] = temp;
        }
        
        return shuffledChoices.GetRange(0, elementCount);
    }

    private static StoryPoint FindNextStoryPointUniqueName(string uniqueName, string choiceText)
    {
        StoryPoint currentStoryPoint = storyPoints[uniqueName];
        string destinationUniqueName = String.Empty;

        if (currentStoryPoint.Type == StoryPointType.Choice || currentStoryPoint.Type == StoryPointType.RChoice)
        {
            foreach (Choice choice in currentStoryPoint.Choices)
            {
                if (choice.TextToShow == choiceText)
                {
                    destinationUniqueName = choice.DestinationStoryPoint;
                    break; // Exit the loop once the matching choice is found
                }
            }
        }
        else
        {
            destinationUniqueName = currentStoryPoint.Choices[0].DestinationStoryPoint;
        }

        if (string.IsNullOrEmpty(destinationUniqueName))
        {
            // Handle the case where the destinationUniqueName is empty
            // You can set a default value or handle it as needed
            // For example, you might want to log a warning or throw an exception
            Debug.LogWarning($"Destination unique name not found for choice text: {choiceText}");
            return null; // Return null to indicate an error
        }

        return storyPoints[destinationUniqueName];
    }
    
    //check for sprite change at each storypoint
    public static void checkForSpriteChange()
    {
        //change sprite
    }
    
    public static void PrintStoryPoints(string fileName = "storyPointDebug.txt")
    {
        MonoBehaviour.print("yoooooooo");
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            sw.WriteLine("------------------------------------");
            foreach (KeyValuePair<string, StoryPoint> keyValuePair in storyPoints)
            {
                sw.WriteLine(keyValuePair.Value.UniqueName + ":");
                sw.WriteLine("  Type: " + keyValuePair.Value.Type);
                sw.WriteLine("  Message: " + keyValuePair.Value.GetTextMessage());
                sw.WriteLine("  Choice count: " + keyValuePair.Value.Choices.Count);
                foreach (Choice choice in keyValuePair.Value.Choices)
                {
                    sw.WriteLine("  Choice text: " + choice.TextToShow);
                    sw.WriteLine("      Choice destination: " + choice.DestinationStoryPoint);
                }
            }
        }
    }
}
