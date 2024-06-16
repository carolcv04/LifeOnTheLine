using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Mover : MonoBehaviour
{
    public Transform PlayerTransform;
    public Backgrounds Backgrounds;
    public TMP_Text Message;
    public GameObject PlayAgainScreenObject;
    
    private string lastChoice;
    private float moveDuration = 2f;
    private Transform currentBackgroundTransform;
    private bool hasShownPlayAgainScreen = false;

    public void Start()
    {
        PlayAgainScreenObject.SetActive(false);
    }
    
    public void Update()
    {
        if (StoryNavigator.IsGameOver() && !hasShownPlayAgainScreen)
            ShowPlayAgainScreen();
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!StoryNavigator.IsGameOver())
                MoveToNextStoryPoint();
        }
    }

    private void ShowPlayAgainScreen()
    {
        hasShownPlayAgainScreen = true;
        PlayAgainScreenObject.SetActive(true);
    }

    private void MoveToNextStoryPoint(string choiceText = null)
    {
        DebugLogger.Log("MOVING TO NEXT POINT");
        StoryNavigator.GetNextStoryPoint(choiceText);
        Message.text = StoryNavigator.GetCurrentMessage();
        StoryNavigator.DisplayChoicesOnButtons();
        String nextSpriteName = StoryNavigator.GetNextBackgroundSpriteName();
        Backgrounds.CreateNextBackground(nextSpriteName);
        StartCoroutine(MoveToNextBackground());
    }

    public void MoveNextChoice(string choiceText = null)
    {
        MoveToNextStoryPoint(choiceText);
    }

    IEnumerator MoveToNextBackground()
    {
        Vector3 startingPosition = PlayerTransform.position;
        float startingY = startingPosition.y; // Store the original y position
        Vector3 endingPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1.5f, 0f, 0f));
        float elapsed = 0.0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            t = Mathf.SmoothStep(0.0f, 1.0f, t);
            Vector3 newPosition = Vector3.Lerp(startingPosition, endingPosition, t);
            newPosition.y = startingY; // Maintain the original y position
            PlayerTransform.position = newPosition;
            elapsed += Time.deltaTime; // Update the elapsed time

            //print("MOVING");

            yield return null; // Wait until the next frame
        }
    }
}
