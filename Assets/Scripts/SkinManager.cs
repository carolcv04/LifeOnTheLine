using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SkinManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<UnityEngine.Sprite> skinsAtStartMenu = new List<UnityEngine.Sprite>();    private List<SpriteRenderer> MaleSprites = new List<SpriteRenderer>();
    private List<SpriteRenderer> FemaleSprites = new List<SpriteRenderer>();
    private List<SpriteRenderer> ThirdGenderSprites = new List<SpriteRenderer>();
    private int selectedSkin = 0;
    public GameObject playerSkin;

    private void matchSpritesToSpriteList()
    {
        
    }
    
    public void NextOption()
    {
        selectedSkin = selectedSkin + 1;
        if (selectedSkin == skinsAtStartMenu.Count)
        {
            selectedSkin = 0;
        }

        sr.sprite = skinsAtStartMenu[selectedSkin];
    }
    
    public void BackOption()
    {
        selectedSkin = selectedSkin - 1;
        if (selectedSkin < 0)
        {
            selectedSkin = skinsAtStartMenu.Count - 1;
        }

        sr.sprite = skinsAtStartMenu[selectedSkin];
    }

    public void PlayGame()
    {
        PrefabUtility.SaveAsPrefabAsset(playerSkin, "Assets/Prefabs/CharacterOne.prefab");
        SceneManager.LoadScene("SampleScene");
    }
    
    //at certain story points, the sprite needs to change.
    /* create a list of all points where the sprite needs to change in the story.
     Using the storynavigator, */
    private string SpriteName01 = StoryNavigator.GetCurrentBackgroundSpriteName();
    
    //set the sprite to whatever the current name is.
    //selected skin = 
}