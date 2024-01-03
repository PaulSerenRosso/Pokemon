using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardPlayerManager : MonoBehaviour
{
    [Header("Player Informations")]
    public List<TMP_Text> playerInformations;
    [Header("Player Badge Arena")]
    public List<Image> playerBadgeArena;
    
    [Header("Look base On Character")]
    public Image playerImage;
    public Image backgroundMenu;
    public List<Sprite> backgroundSprites = new List<Sprite>();
    public List<Sprite> playerSprites = new List<Sprite>();

    [Header("Debug")]
    [Range(0, 1)] 
    public int playerCharacter;
    [Range(0, 8)] 
    public int playerBadge;
    
    [Range(0, 100)] 
    public int seed;

    private int idNo;
    private int totalFramesPlayed;
    const int framesPerSecond = 300;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerID(idNo);
        SetPlayerName("JOE");
        SetPlayerMoney(1000);
        SetPokedexCount(0);
        
        EmptyListImage(playerBadgeArena);
    }

    void Update()
    {
        idNo = CalculateIDNo(seed);
        SetPlayerID(idNo);

        // Increment frame count each frame
        totalFramesPlayed += 1;

        for (int i = 0; i < playerBadgeArena.Count; i++)
        {
            // Set the alpha value based on the playerBadge value
            float alpha = (playerBadge >= i + 1) ? 1.0f : 0.0f;
            UpdateImageAlpha(playerBadgeArena[i], alpha);
        }

        // Display the time played
        DisplayTimePlayed();
    }

    void DisplayTimePlayed()
    {
        // Call the function to get the time played
        string timePlayed = CalculateTimePlayed(totalFramesPlayed);

        // Set the time played text
        SetTimePlay(timePlayed);
    }

    void SetTimePlay(string time)
    { 
        SetTexTInfo(4, time);
    }

    string CalculateTimePlayed(int totalFrames)
    {
        // Calculate total time in seconds
        float totalTimeInSeconds = (float)totalFrames / framesPerSecond;

        // Calculate hours, minutes, and seconds
        int hours = (int)(totalTimeInSeconds / 3600);
        int minutes = (int)((totalTimeInSeconds % 3600) / 60);
        //int seconds = (int)(totalTimeInSeconds % 60);


        // Format the time into a string
        string formattedTime = string.Format("{0:D2}:{1:D2}", hours, minutes);
        //string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);


        return formattedTime;
    }


    void UpdateImageAlpha(Image image, float alpha)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }
    }

    public void SetPlayerID(int number)
    {
        SetTexTInfo(0, number.ToString());
    }

    public void SetPlayerName(string name)
    {
        SetTexTInfo(1, "NAME :" + " " + name);
    }
    
    public void SetPlayerMoney(int number)
    {
        SetTexTInfo(2, number.ToString());
    }
    
    public void SetPokedexCount(int number)
    {
        SetTexTInfo(3, number.ToString());
    }

    void SetTexTInfo(int index, string name)
    {
        playerInformations[index].text = name;
    }
    
    private void SwitchBackgroundBaseOnCharacter(int index)
    {
        if (index == 0)
        {
            playerImage.sprite = playerSprites[0];
            backgroundMenu.sprite = backgroundSprites[0];
        }
        else
        {
            playerImage.sprite = playerSprites[1];
            backgroundMenu.sprite = backgroundSprites[1];
        }
    }

    public static int CalculateIDNo(int seed)
    {
        const int multiplier = 0x41C64E6D;
        const int addition = 0x00006073;

        int result = multiplier * seed + addition;
        return (result & 0x7FFFFFFF) % 65536;
    }

    public void EmptyListImage(List<Image> images)
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                Color color = image.color;
                color.a = 0f;
                image.color = color;
            }
        }
    }
}