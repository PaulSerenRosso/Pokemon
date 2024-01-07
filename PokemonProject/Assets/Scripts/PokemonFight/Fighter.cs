using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    public List<Pokemon> pokemons = new List<Pokemon>();
    private Fighter enemyFighter;
    private bool hasLost;
    public Action endTurnEvent;
    public Action chooseActionEvent;
    public int currentPokemonIndex;
    public Action initEvent;
    public Action disableEvent;
    private SpriteRenderer fighterSpriteRenderer;
    private SpriteRenderer enemySpriteRenderer;
    private TextMeshProUGUI nameText;
    private Slider hpSlider;
    private Image statusBackground;
    private TextMeshProUGUI statusText;
    protected virtual void Start()
    {
      
    }
    
    public virtual void Init(Fighter enemyFighter, SpriteRenderer fighterSpriteRenderer, SpriteRenderer enemySpriteRenderer, Slider hpSlider, TextMeshProUGUI nameText, TextMeshProUGUI statusText, Image statusBackground )
    {
        this.enemyFighter = enemyFighter;
        hasLost = false;
        currentPokemonIndex = 0;
        initEvent?.Invoke();
        this.fighterSpriteRenderer = fighterSpriteRenderer;
        this.enemySpriteRenderer = enemySpriteRenderer;
        this.hpSlider = hpSlider;
        this.nameText = nameText;
        nameText.text = pokemons[currentPokemonIndex].so.name;
        this.fighterSpriteRenderer.sprite = pokemons[currentPokemonIndex].so.battleSprite;
        this.statusBackground = statusBackground;
        this.statusText = statusText;
    }

    public virtual void Disable()
    {
        disableEvent?.Invoke();
    }

    public virtual bool CheckLose()
    {
        if (CheckPokemonsAreAllDead())
        {
            return true;
        }
        return false;
    }
    
    private bool CheckPokemonsAreAllDead()
    {
        bool areAllDead = true;
        for (var index = 0; index < pokemons.Count; index++)
        {
            var pokemon = pokemons[index];
            if (!pokemon.IsDied)
            {
                areAllDead = false;
                break;
            }
        }

        return areAllDead;
    }

    public void UseCapacityFeedback(int index, MonoBehaviour coroutineHandler)
    {
        if (CheckUseCapacityStatus())
        {
            chooseActionEvent?.Invoke();
            TriggerStatus();
        }
        else
        {
            pokemons[currentPokemonIndex].capacities[index].useCapacityFeedbackFinished = () => UseCapacity(index);
            if (pokemons[currentPokemonIndex].capacities[index]
                .TryUseCapacityFeedback(fighterSpriteRenderer, enemySpriteRenderer, coroutineHandler))
            {
          
                chooseActionEvent?.Invoke();
            };
        }
    }
    
    public void UseCapacity(int index)
    {
        pokemons[currentPokemonIndex].capacities[index].TryUseCapacity(pokemons[currentPokemonIndex], enemyFighter.pokemons[enemyFighter.currentPokemonIndex]);
        if (pokemons[currentPokemonIndex].capacities[index].TryUseStatus() &&
            enemyFighter.pokemons[enemyFighter.currentPokemonIndex].currentStatus != null)
        {
            enemyFighter.AddStatus(pokemons[currentPokemonIndex].capacities[index].pokemonStatusType);
        }
        else
        {
            if (CheckTriggerEndTurnStatus())
            {
                TriggerStatus();
            }
            else
            {
                endTurnEvent?.Invoke();
            }
        }
       
    }

    public void RefreshRenderer()
    {
        hpSlider.value = (float)pokemons[currentPokemonIndex].Hp / pokemons[currentPokemonIndex].MaxHp;
    }

    public bool CheckTriggerEndTurnStatus()
    {
        if (pokemons[currentPokemonIndex].currentStatus != null )
        {
            if (pokemons[currentPokemonIndex].currentStatus.isTriggerAfterTurn)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckUseCapacityStatus()
    {
        if (pokemons[currentPokemonIndex].currentStatus != null )
        {
            if (!pokemons[currentPokemonIndex].currentStatus.isTriggerAfterTurn)
            { 
                return true;
            }
        }

        return false;
    }

    public void AddStatus(PokemonStatusType statusType)
    {
        switch (statusType)
        {
            case PokemonStatusType.Burn:
            {
                pokemons[currentPokemonIndex].currentStatus = new BurnStatus(pokemons[currentPokemonIndex]);
                break;
            }
            case PokemonStatusType.Freeze:
            {
                pokemons[currentPokemonIndex].currentStatus = new FreezeStatus(pokemons[currentPokemonIndex]);
                break;
            }
            case PokemonStatusType.Paralysis:
            {
                pokemons[currentPokemonIndex].currentStatus = new ParalysisStatus(pokemons[currentPokemonIndex]);
                break;
            }
            case PokemonStatusType.Poison:
            {
                pokemons[currentPokemonIndex].currentStatus = new PoisonStatus(pokemons[currentPokemonIndex]);
                break;
            }
            case PokemonStatusType.Sleep:
            {
                pokemons[currentPokemonIndex].currentStatus = new SleepStatus(pokemons[currentPokemonIndex]);
                break;
            }
        }
        pokemons[currentPokemonIndex].currentStatus.useCapacityFeedbackFinished = AddUIStatus;
        pokemons[currentPokemonIndex].currentStatus.TriggerStatusFeedback(fighterSpriteRenderer, this);
    }

    private void AddUIStatus()
    {
        statusBackground.gameObject.SetActive(true);
        statusText.gameObject.SetActive(true);
        statusText.text = pokemons[currentPokemonIndex].currentStatus.statusText;
        statusBackground.color = pokemons[currentPokemonIndex].currentStatus.statusColor;
        endTurnEvent?.Invoke();
    }

    private void TriggerStatus()
    {
        
    }

    public void RemoveStatus()
    {
        statusBackground.gameObject.SetActive(false);
        statusText.gameObject.SetActive(false);
    }
    
    

  
}
