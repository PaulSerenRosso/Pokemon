using System;
using SequencerNS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private GameObject choiceHolder;
    public Button[] buttons = new Button[4];

    private void Start()
    {
        choiceHolder.SetActive(false);
    }

    public void PopChoiceInteraction(ChoiceSO choice)
    {
        Debug.Log("PopChoiceInteraction");
        ResetButtons();
        
        //choiceHolder.SetActive(true);
        
        Debug.Log(choice.resolutions.Length);
        
        for (int i = 0; i < choice.resolutions.Length; i++)
        {
            // Setup Buttons
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = choice.resolutions[i].title;
            
            // Add actions
            var i1 = i;
            buttons[i].onClick.AddListener(() => SetResolution(choice.resolutions[i1]));
            buttons[i].onClick.AddListener(() => OnEndChoiceOnInteraction());
        }
        
        // Draw
        choiceHolder.SetActive(true);
    }

    private void SetResolution(ResolutionSO resolutionSo)
    {
        if (resolutionSo.textToDraw[0] != null)
        {
            Sequencer.Instance.dialogueManager.ReadText(resolutionSo.textToDraw[0], Gender.Annoucement);
        }
        
        switch (resolutionSo.resolutionType)
        {
            case ResolutionSO.ResolutionsInteractions.CloseBox: break;
            case ResolutionSO.ResolutionsInteractions.PokemonGiver: Sequencer.Instance.character.GetComponent<PlayerFighter>().AddPokemon(resolutionSo.pokemonToGive); break;
            //case ResolutionSO.ResolutionsInteractions.SetName: break;
            //case ResolutionSO.ResolutionsInteractions.SetGender: break;
            default: throw new ArgumentOutOfRangeException();
        }
    }

    private void ResetButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
            buttons[i].onClick.RemoveAllListeners();
        }
    }
    
    private void OnEndChoiceOnInteraction()
    {
        choiceHolder.SetActive(false);
        Sequencer.Instance.OnEndInteraction();
    }
}
