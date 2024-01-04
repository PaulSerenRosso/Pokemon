using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonTeamManager : MonoBehaviour
{
    [Header("Player Pokemon Team")] 
    public List<PokemonSO> pokemonTeam = new List<PokemonSO>();
    public List<GameObject> pokemonDisplayOnTeams = new List<GameObject>();
    
    [Header("Cursor Selected Pokemon")]
    public List<Image> backgroundPokemonImagesNotSelected = new List<Image>();
    public List<Image> backgroundPokemonImagesSelected = new List<Image>();
    public List<Image> swapPokeballCancelButton = new List<Image>();
    
    [Header("Pokemon Option Menu")]
    public GameObject pokemonOptionMenu;
    public List<Image> arrowPositionsPokemonOptionMenu = new List<Image>();
    private int currentArrowPositionPokemonOptionMenu = 0;
    private bool isPokemonOptionMenuActive = false;
    
    [Header("Pokemon Rendering Part")]
    public List<TMP_Text> pokemonNameTexts = new List<TMP_Text>();
    public List<TMP_Text> pokemonLevels = new List<TMP_Text>();
    public List<TMP_Text> pokemonCurrentsHP = new List<TMP_Text>();
    public List<TMP_Text> pokemonMaxsHP = new List<TMP_Text>();
    public List<Image> pokemonImages = new List<Image>();
    public List<Image> pokemonSexImages= new List<Image>();
    public TMP_Text pokemonDescriptionActionText;
    public string[] pokemonDescriptionActionTexts = new []{"Choose a POKEMON", "Do what with this PKMN ?"};
    
    [Header("Pokemon Sex")]
    public List<Sprite> pokemonSexSprites = new List<Sprite>();
    
    [Header("Debug")]
    [Range(0,1)]
    public int currentPokemonWithPlayer = 0;
    public int currentPokemonIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        //Enable Pokemon that are in the Team
        EnablePokemonDisplaysAndUpdateUI();
    }
    
    

    void Update()
    {
        HandlePokemonSelected();
        HandleButtonAInput();
        UpdateBackgroundImagesSelection();
        UpdateSwapPokeballCancelButton();
        EnablePokemonDisplaysAndUpdateUI();
    }
    
    void UpdateUIForCurrentPokemon()
    {
        UpdateUIForPokemonAtIndex(currentPokemonIndex);
    }
    
    private void EnablePokemonDisplaysAndUpdateUI()
    {
        // Disable all displays first
        DisableListOfGO(pokemonDisplayOnTeams);

        // Enable displays up to the count of the Pokemon team
        for (int i = 0; i < pokemonTeam.Count && i < pokemonDisplayOnTeams.Count; i++)
        {
            pokemonDisplayOnTeams[i].SetActive(true);

            // Update UI for the enabled Pokemon display
            UpdateUIForPokemonAtIndex(i);
        }
    }
    void RefreshUIBasedOnTeam()
    {
        EnablePokemonDisplaysAndUpdateUI();
    }

    private void UpdateUIForPokemonAtIndex(int index)
    {
        if (index < 0 || index >= pokemonTeam.Count)
        {
            Debug.LogError("Invalid index for updating UI");
            return;
        }

        // NAME
        pokemonNameTexts[index].text = pokemonTeam[index].name;
        // CurrentLevel
        pokemonLevels[index].text = pokemonTeam[index].startLevel.ToString();
        // TODO : CurrentHP
        //pokemonCurrentsHP[index].text = pokemonTeam[index].maxHp.ToString();
        // MaxHP
        pokemonMaxsHP[index].text = pokemonTeam[index].maxHp.ToString();
        // Icon Pokemon Team
        pokemonImages[index].sprite = pokemonTeam[index].teamSprite;
        //Icon Sex
        pokemonSexImages[index].sprite = pokemonSexSprites[pokemonTeam[index].pokemonSex];
    }
    
    public void EnableListOfGO(List<GameObject> gos)
    {
        foreach (GameObject go in gos)
        {
            go.gameObject.SetActive(true);
        }
    } 
    
    
    
    private void HandlePokemonSelected()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!isPokemonOptionMenuActive)
            {
                currentPokemonIndex++;
                if(currentPokemonIndex > GetPokemonTeamCount())
                {
                    currentPokemonIndex = 0;
                }    
            }
            else
            {
                currentArrowPositionPokemonOptionMenu++;
                if(currentArrowPositionPokemonOptionMenu > GetOptionMenuCount())
                {
                    currentArrowPositionPokemonOptionMenu = 0;
                }
                UpdateArrowPositionPokemonOptionMenu();
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isPokemonOptionMenuActive)
            {
                currentPokemonIndex--;
                if(currentPokemonIndex < 0)
                {
                    currentPokemonIndex = GetPokemonTeamCount();
                }
            }
            else
            {
                currentArrowPositionPokemonOptionMenu--;
                if(currentArrowPositionPokemonOptionMenu < 0)
                {
                    currentArrowPositionPokemonOptionMenu = GetOptionMenuCount() - 1;
                }
                UpdateArrowPositionPokemonOptionMenu();
            }
        }
    }
    
    void HandleButtonAInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (isPokemonOptionMenuActive)
            {
                // Handle arrow index only if the option menu is active
                HandleArrowIndex();
            }
            else
            {
                // Rest of your existing logic when the A button is pressed for the first time
                if (currentPokemonIndex != GetPokemonTeamCount())
                {
                    DisplayPokemonOptionMenu();
                }
                else
                {
                    HandleCancelButton();
                }
            }

            // Update text description regardless of the option menu state
            ChangeTextDescriptionAction(isPokemonOptionMenuActive
                ? pokemonDescriptionActionTexts[1]
                : pokemonDescriptionActionTexts[0]);
        }
    }

    void HandleArrowIndex()
    {
        switch (currentArrowPositionPokemonOptionMenu)
        {
            case 0:
                Debug.Log("Summary Menu");
                break;
            case 1:
                HandleSwitchPokemon();
                break;
            case 2:
                Debug.Log("Inventory Menu");
                break;
            case 3:
                HandleCancelButtonPokemonOptionMenu();
                break;
        }
    }
    
    private void UpdateBackgroundImagesSelection()
    {
        if (currentPokemonIndex < 0 || currentPokemonIndex >= backgroundPokemonImagesSelected.Count)
        {
            Debug.LogError("Invalid index for updating background images selection");
            return;
        }

        foreach (Image image in backgroundPokemonImagesSelected)
        {
            image.gameObject.SetActive(false);
        }

        if (currentPokemonIndex < GetPokemonTeamCount())
        {
            backgroundPokemonImagesSelected[currentPokemonIndex].gameObject.SetActive(true);
        }
        else
        {
            backgroundPokemonImagesSelected[6].gameObject.SetActive(true);
        }
    }
    
    private void UpdateSwapPokeballCancelButton()
    {
        if (currentPokemonIndex < GetPokemonTeamCount())
        {
            SetSwapPokeballCancelButtonActiveState(true, false);
        }
        else
        {
            SetSwapPokeballCancelButtonActiveState(false, true);
        }
    }

    private void SetSwapPokeballCancelButtonActiveState(bool index0Active, bool index1Active)
    {
        swapPokeballCancelButton[0].gameObject.SetActive(index0Active);
        swapPokeballCancelButton[1].gameObject.SetActive(index1Active);
    }
    
    private int GetPokemonTeamCount()
    {
        return pokemonTeam.Count;
    }
    
    private int GetOptionMenuCount()
    {
        return arrowPositionsPokemonOptionMenu.Count - 1;
    }
    
    public void DisableListOfGO(List<GameObject> gos)
    {
        foreach (GameObject go in gos)
        {
            go.gameObject.SetActive(false);
        }
    }    
    
    public void EnableOrDisableListOfGO(List<GameObject> gos, bool state = true)
    {
        foreach (GameObject go in gos)
        {
            go.gameObject.SetActive(state);
        }
    }
    
    public void ChangeTextDescriptionAction(string text)
    {
        pokemonDescriptionActionText.text = text;
    }
    
    public void DisplayPokemonOptionMenu()
    {
        pokemonOptionMenu.SetActive(true);
        isPokemonOptionMenuActive = true;
        currentArrowPositionPokemonOptionMenu = 0;
        UpdateArrowPositionPokemonOptionMenu();
    }

    private void UpdateArrowPositionPokemonOptionMenu()
    {
        foreach (Image arrowPosition in arrowPositionsPokemonOptionMenu)
        {
            // Set alpha to 0 for all arrow positions
            Color color = arrowPosition.color;
            color.a = 0f;
            arrowPosition.color = color;
        }

        // Set alpha to 1 for the current arrow position
        Color currentArrowColor = arrowPositionsPokemonOptionMenu[currentArrowPositionPokemonOptionMenu].color;
        currentArrowColor.a = 1f;
        arrowPositionsPokemonOptionMenu[currentArrowPositionPokemonOptionMenu].color = currentArrowColor;
    }
    
    public void SwitchPokemon(int index1, int index2)
    {
        if (index1 < 0 || index1 >= pokemonTeam.Count || index2 < 0 || index2 >= pokemonTeam.Count)
        {
            Debug.LogError("Invalid indices for switching Pokemon.");
            return;
        }

        // Swap Pokemon at index1 and index2
        (pokemonTeam[index1], pokemonTeam[index2]) = (pokemonTeam[index2], pokemonTeam[index1]);

        // Update UI for the switched Pokemon
        UpdateUIForPokemonAtIndex(index1);
        UpdateUIForPokemonAtIndex(index2);
    }
    
    void HandleSwitchPokemon()
    {
        if (GetPokemonTeamCount() >= 2)
        {
            int nextPokemonIndex = (currentPokemonIndex + 1) % GetPokemonTeamCount();
            SwitchPokemon(currentPokemonIndex, nextPokemonIndex);

            UpdateUIForPokemonAtIndex(currentPokemonIndex);
            UpdateUIForPokemonAtIndex(nextPokemonIndex);

            currentPokemonIndex = nextPokemonIndex;
        }
        else
        {
            // Handle the case when there are not enough Pokemon in the team
            Debug.Log("Not enough Pokemon to switch.");
        }
    }

    void HandleCancelButtonPokemonOptionMenu()
    {
        pokemonOptionMenu.SetActive(false);
        isPokemonOptionMenuActive = false;

        if (currentArrowPositionPokemonOptionMenu == 1)
        {
            currentPokemonIndex = (currentPokemonIndex + 1) % GetPokemonTeamCount();
        }
    }

    public void HandleCancelButton()
    {
        ToggleOffThePokemonTeamMenu();
    }
    
    public void ToggleOffThePokemonTeamMenu()
    {
        gameObject.SetActive(false);
    }
    
}
