using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    public GameObject BackgroundPrefab;
    public UnityEngine.Sprite DefaultBackgroundSprite; // Change Sprite to UnityEngine.Sprite
    public List<String> BackgroundNames;
    public List<UnityEngine.Sprite> BackgroundSprites; // Change Sprite to UnityEngine.Sprite

    private Vector3 currentBackgroundPosition;

    public void Start()
    {
        currentBackgroundPosition = new Vector3(0f, 0f, 0f);
    }

    public void CreateNextBackground(String name)
    {
        UnityEngine.Sprite newSprite; // Change Sprite to UnityEngine.Sprite
        if (!BackgroundNames.Contains(name))
        {
            newSprite = DefaultBackgroundSprite;
        }
        else
        {
            newSprite = BackgroundSprites[BackgroundNames.IndexOf(name)];
        }

        Vector3 nextBackgroundPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width *1.5f, Screen.height/2, 0f ));

        GameObject newObject = Instantiate(BackgroundPrefab, nextBackgroundPosition,
            Quaternion.identity);
        SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newSprite;
        
        currentBackgroundPosition = nextBackgroundPosition;
    }
}