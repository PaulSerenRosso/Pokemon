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
    
    [Header("Pokemon Rendering Part")]
    public List<TMP_Text> pokemonNameTexts = new List<TMP_Text>();
    public List<TMP_Text> pokemonLevels = new List<TMP_Text>();
    public List<TMP_Text> pokemonCurrentsHP = new List<TMP_Text>();
    public List<TMP_Text> pokemonMaxsHP = new List<TMP_Text>();
    public List<Image> pokemonImages = new List<Image>();
    public List<Image> pokemonSexImages= new List<Image>();
    public TMP_Text pokemonDescriptionActionText;
    
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
        HandleArrowInput();
        UpdateBackgroundImagesSelection();
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
    
    
    
    private void HandleArrowInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentPokemonIndex++;
            if(currentPokemonIndex > 6)
            {
                currentPokemonIndex = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentPokemonIndex--;
            if(currentPokemonIndex < 0)
            {
                currentPokemonIndex = 6;
            }
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
        backgroundPokemonImagesSelected[currentPokemonIndex].gameObject.SetActive(true);
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
}
