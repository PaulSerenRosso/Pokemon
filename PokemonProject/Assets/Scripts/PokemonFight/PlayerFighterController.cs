using System;
using SequencerNS;
using TMPro;
using UnityEngine;

public class PlayerFighterController : FighterController
{
    [SerializeField] private GameObject choiceCapacityPanel;
    [SerializeField] private GameObject choiceActionPanel;
    [SerializeField] private GameObject capacityInfoPanel;
    [SerializeField] private TextMeshProUGUI[] allCapacitiesText;
    [SerializeField] private TextMeshProUGUI capacityTypeText;
    [SerializeField] private TextMeshProUGUI capacityStaminaText;
    [SerializeField] private GameObject bagPanel;
    [SerializeField] private GameObject pokemonTeamPanel;
    public Action fleeEvent;


    public void ActivateChoiceCapacityPanel()
    {
        choiceActionPanel.SetActive(false);
        choiceCapacityPanel.SetActive(true);
        capacityInfoPanel.SetActive(true);
        for (int i = 0; i < allCapacitiesText.Length; i++)
        {
            allCapacitiesText[i].gameObject.SetActive(false);
        }
        var pokemonCapacities = fighter.pokemons[fighter.currentPokemonIndex].so.pokemonCapacitySO;
        for (var index = 0; index < pokemonCapacities.Length; index++)
        {
            var capacityText = allCapacitiesText[index];
            capacityText.gameObject.SetActive(true);
            capacityText.text = pokemonCapacities[index].name;
        }
        UpdateCapacityInfoPanel(0);
    }

    public void UpdateCapacityInfoPanel(int index)
    {
        capacityTypeText.text = fighter.pokemons[fighter.currentPokemonIndex].so.pokemonCapacitySO[index].pokemonTypeSo.name;
        capacityStaminaText.text = fighter.pokemons[fighter.currentPokemonIndex].capacities[index].Stamina.ToString();
    }
    
    public override void Activate()
    {
        choiceActionPanel.SetActive(true);
    }

    public override void Deactivate()
    {
        choiceCapacityPanel.SetActive(false);
        choiceActionPanel.SetActive(false);
        capacityInfoPanel.SetActive(false);
    }

    public void UseCapacity(int index)
    {
    
  
                fighter.UseCapacityFeedback(index);
    }

    public void Flee()
    {
        fleeEvent?.Invoke();
    }
    public void OpenBag()
    {
        bagPanel.SetActive(true);
    }
    
    public void OpenPokemonTeam()
    {
        pokemonTeamPanel.SetActive(true);
    }
}
