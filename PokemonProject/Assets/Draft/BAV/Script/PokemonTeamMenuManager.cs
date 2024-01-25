using System;
using System.Collections;
using System.Collections.Generic;
using SequencerNS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokemonTeamManager : MonoBehaviour
{
    [Header("Player Pokemon Team")] 
    public List<GameObject> pokemonDisplayOnTeams = new List<GameObject>();
    
    [Header("Cursor Selected Pokemon")]
    public List<Image> backgroundPokemonImagesNotSelected = new List<Image>();
    public List<Image> backgroundPokemonImagesSelected = new List<Image>();
    public List<Image> swapPokeballCancelButton = new List<Image>();
    
    [Header("Pokemon Option Menu")]
    public GameObject pokemonOptionMenu;
    public List<Image> arrowImagesPokemonOptionMenu = new List<Image>();
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
    [SerializeField] private FightManager fightManager;
    
    private Action<Pokemon> pokemonSelectedAction;

    private bool inUsingItem = false;
    private bool currentPokemonInFightIsDead = false;
    // Start is called before the first frame update
    void Start()
    {
        //Enable Pokemon that are in the Team
        EnablePokemonDisplaysAndUpdateUI();
    }

   public  void ActivateAndDeactivateCancelButton(bool state = false)
    {
        
        currentPokemonInFightIsDead = state;
        SetSwapPokeballCancelButtonActiveState(state, state);
    }

    void Update()
    {
        HandlePokemonSelected();
        HandleButtonAInput();
        UpdateBackgroundImagesSelection();
        UpdateSwapPokeballCancelButton();
        EnablePokemonDisplaysAndUpdateUI();
    }

    public void OpenPokemonTeamMenuForApplyItem(Action<Pokemon> callback)
    {
        pokemonSelectedAction = callback;
        inUsingItem = true;
        gameObject.SetActive(true);
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
        for (int i = 0; i < PlayerManager.Instance.playerFighter.pokemons.Count && i < pokemonDisplayOnTeams.Count; i++)
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
        if (index < 0 || index >= PlayerManager.Instance.playerFighter.pokemons.Count)
        {
            Debug.LogError("Invalid index for updating UI");
            return;
        }

        // NAME
        pokemonNameTexts[index].text =PlayerManager.Instance.playerFighter.pokemons[index].so.name;
        // CurrentLevel
        pokemonLevels[index].text = PlayerManager.Instance.playerFighter.pokemons[index].Level.ToString();
        // TODO : CurrentHP
        pokemonCurrentsHP[index].text =PlayerManager.Instance.playerFighter.pokemons[index].Hp.ToString();
        // MaxHP
        pokemonMaxsHP[index].text = PlayerManager.Instance.playerFighter.pokemons[index].MaxHp.ToString();
        // Icon Pokemon Team
        pokemonImages[index].sprite = PlayerManager.Instance.playerFighter.pokemons[index].so.teamSprite;
        //Icon Sex
        pokemonSexImages[index].sprite = pokemonSexSprites[PlayerManager.Instance.playerFighter.pokemons[index].so.pokemonSex];
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
        if (Input.GetKeyDown(KeyCode.A) && Sequencer.Instance.CurrentSequenceType == SequenceType.None)
        {
            if (isPokemonOptionMenuActive)
            {
                // Handle arrow index only if the option menu is active
                HandleArrowIndex();
            }
            else
            {
                if (currentPokemonIndex != GetPokemonTeamCount())
                {
                    DisplayPokemonOptionMenu();
                }
                else
                {
                    if (!currentPokemonInFightIsDead)
                    {
                   
                        HandleCancelButton();
                    }
                    else
                    {
                        
                    }
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
        return PlayerManager.Instance.playerFighter.pokemons.Count;
    }
    
    private int GetOptionMenuCount()
    {
        return arrowImagesPokemonOptionMenu.Count - 1;
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
        if (inUsingItem)
        {
            if (PlayerManager.Instance.playerFighter.pokemons[currentPokemonIndex].IsDied && 
                PlayerManager.Instance.playerFighter.pokemons[currentPokemonIndex].Hp != PlayerManager.Instance.playerFighter.pokemons[currentPokemonIndex].MaxHp)
            {
                return;
            }
            pokemonSelectedAction?.Invoke(PlayerManager.Instance.playerFighter.pokemons[currentPokemonIndex]);
        }
        else
        {
            pokemonOptionMenu.SetActive(true);
            isPokemonOptionMenuActive = true;
            currentArrowPositionPokemonOptionMenu = 0;
            UpdateArrowPositionPokemonOptionMenu(); 
        }

    }

    private void UpdateArrowPositionPokemonOptionMenu()
    {
        foreach (Image arrowPosition in arrowImagesPokemonOptionMenu)
        {
            // Set alpha to 0 for all arrow positions
            Color color = arrowPosition.color;
            color.a = 0f;
            arrowPosition.color = color;
        }

        // Set alpha to 1 for the current arrow position
        Color currentArrowColor = arrowImagesPokemonOptionMenu[currentArrowPositionPokemonOptionMenu].color;
        currentArrowColor.a = 1f;
        arrowImagesPokemonOptionMenu[currentArrowPositionPokemonOptionMenu].color = currentArrowColor;
    }
    
    public void SwitchPokemon(int index1, int index2)
    {
        if (index1 < 0 || index1 >= PlayerManager.Instance.playerFighter.pokemons.Count || index2 < 0 || index2 >=PlayerManager.Instance.playerFighter.pokemons.Count)
        {
            Debug.LogError("Invalid indices for switching Pokemon.");
            return;
        }
        // Swap Pokemon at index1 and index2
        (PlayerManager.Instance.playerFighter.pokemons[index1], PlayerManager.Instance.playerFighter.pokemons[index2]) = (PlayerManager.Instance.playerFighter.pokemons[index2], PlayerManager.Instance.playerFighter.pokemons[index1]);

        // Update UI for the switched Pokemon
        UpdateUIForPokemonAtIndex(index1);
        UpdateUIForPokemonAtIndex(index2);
        if (fightManager.isInFight)
        { 
            fightManager.ResolveFight();
            pokemonOptionMenu.SetActive(false);
            gameObject.SetActive(false);
            Sequencer.Instance.AddCombatInteraction($"{fightManager.playerFighterController.fighter.GetCurrentPokemonName()} go !", true,
                () =>
                {
                    if (currentPokemonInFightIsDead)
                    {
                        fightManager.playerFighterController.fighter.RefreshRenderer();
                        fightManager.ChangeTurn();
                    }
                    else
                    {
                        fightManager.playerFighterController.fighter.EndOwnTurn();
                    }
                    
                });
        }
    }
    
    void HandleSwitchPokemon()
    {
        if (GetPokemonTeamCount() >= 2)
        {
    
            if(!PlayerManager.Instance.playerFighter.pokemons[currentPokemonIndex].IsDied) 
            {
                if (currentPokemonIndex == 0)
                {
                    Sequencer.Instance.AddCombatInteraction("Select another Pokemon than the current one activated", false,()=>{});
                    return;
                }
                SwitchPokemon(currentPokemonIndex, 0);
                UpdateUIForPokemonAtIndex(currentPokemonIndex);
                UpdateUIForPokemonAtIndex(0);
                currentPokemonIndex = 0;
                return;
            }
            else
            {
                Sequencer.Instance.AddCombatInteraction("The Pokemons need to be alive", false, ()=>{});
            }
        }
        else
        {
            Sequencer.Instance.AddCombatInteraction("You need more than one Pokemon", false,()=>{});
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
    
    private void OnEnable()
    {
        Sequencer.Instance.OnEndSequence = null;
    }

    private void OnDisable()
    {
        Sequencer.Instance.OnEndSequence = null;
    }
}
