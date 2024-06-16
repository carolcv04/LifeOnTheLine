using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoryPointType
{
    Event,
    REvent,
    Choice,
    RChoice,
    Ending,
    Undefined
}

public class StoryPoint
{
    public StoryPointType Type;
    public string UniqueName;
    public string Text;
    public string BackgroundSpriteName;
    public List<Choice> Choices  = new List<Choice>();
    public int RandomCount;

    public StoryPoint(StoryPointType type, string uniqueName, string text, string backgroundSpriteName, 
        int randomCount = 0)
    {
        Type = type;
        UniqueName = uniqueName;
        Text = text;
        BackgroundSpriteName = backgroundSpriteName;
        RandomCount = randomCount;
    }

    public string GetTextMessage()
    {
        return Text;
    }

    public string GetBackgroundSpriteName()
    {
        return BackgroundSpriteName;
    }
        
    public List<string> GetChoices()
    {
        List<string> choices = new List<string>();
        foreach (Choice choice in Choices)
        {
            choices.Add(choice.TextToShow);
        }

        return choices;
    }
}
