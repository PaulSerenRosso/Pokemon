using System;
using System.Collections.Generic;
using SequencerNS;
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
    private MonoBehaviour coroutineHandler;
    public bool canCapturedPokemons;
    public string pokemonAdjective = "";
    
    protected virtual void Start()
    {
      
    }
    
    public virtual void Init(Fighter enemyFighter, SpriteRenderer fighterSpriteRenderer, SpriteRenderer enemySpriteRenderer, Slider hpSlider, TextMeshProUGUI nameText, TextMeshProUGUI statusText, Image statusBackground, MonoBehaviour coroutineHandler )
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
        this.coroutineHandler = coroutineHandler;
    }

    public virtual void Disable()
    {
        disableEvent?.Invoke();
    }

    public void CheckPokemonIsDead()
    {
        if (pokemons[currentPokemonIndex].IsDied)
        {
            Sequencer.Instance.AddCombatInteraction($"{GetCurrentPokemonName()} + is K.O", () =>
            {
                if (CheckPokemonsAreAllDead())
                {
                   endTurnEvent?.Invoke();
                }
                else
                {
                    
                    // sinon lancer le 
                }
            });
        }
    }

    private string GetCurrentPokemonName()
    {
        return $"{pokemons[currentPokemonIndex].so.name}{pokemonAdjective}";
    }
    public virtual bool CheckLose()
    {
        if (CheckPokemonsAreAllDead())
        {
            return true;
        }

        if (pokemons.Count == 0)
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

    public void UseCapacityFeedback(int index)
    {
        Sequencer.Instance.AddCombatInteraction(
            $"{GetCurrentPokemonName()} uses {pokemons[currentPokemonIndex].capacities[index].so.name} ", () =>
            {
                if (CheckUseCapacityStatus())
                {
                    chooseActionEvent?.Invoke();
                    TriggerStatusFeedback();
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
            });
    }
    
    public void UseCapacity(int index)
    {
        pokemons[currentPokemonIndex].capacities[index].TryUseCapacity(pokemons[currentPokemonIndex], enemyFighter.pokemons[enemyFighter.currentPokemonIndex]);
        if (pokemons[currentPokemonIndex].capacities[index].TryUseStatus() &&
            enemyFighter.pokemons[enemyFighter.currentPokemonIndex].currentStatus == null)
        {
            enemyFighter.AddStatus(pokemons[currentPokemonIndex].capacities[index].pokemonStatusType);
        }
        else
        {
            if (CheckTriggerEndTurnStatus())
            {
                TriggerStatusFeedback();
            }
            else
            {
               CheckPokemonIsDead();
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
                if (CheckStatusCondition()) return true;
            }
        }

        return false;
    }

    private bool CheckStatusCondition()
    {
        if (pokemons[currentPokemonIndex].currentStatus.CheckCanTriggerStatus())
        {
            return true;
        }

        if (pokemons[currentPokemonIndex].currentStatus.isEndStatus)
        {
            RemoveStatus();
        }

        return false;
    }

    public bool CheckUseCapacityStatus()
    {
        if (pokemons[currentPokemonIndex].currentStatus != null )
        {
            if (!pokemons[currentPokemonIndex].currentStatus.isTriggerAfterTurn)
            { 
                
                if (CheckStatusCondition()) return true;
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
        //TODO : fix le bug qui a la 
        pokemons[currentPokemonIndex].currentStatus.useCapacityFeedbackFinished = AddUIStatus;
        pokemons[currentPokemonIndex].currentStatus.TriggerStatusFeedback(fighterSpriteRenderer, coroutineHandler);
    }

    private void AddUIStatus()
    {
        statusBackground.gameObject.SetActive(true);
        statusText.gameObject.SetActive(true);
        statusText.text = pokemons[currentPokemonIndex].currentStatus.statusText;
        statusBackground.color = pokemons[currentPokemonIndex].currentStatus.statusColor;
        CheckPokemonIsDead();
    }

    private void TriggerStatusFeedback()
    {
        pokemons[currentPokemonIndex].currentStatus.useCapacityFeedbackFinished = TriggerStatus;
        pokemons[currentPokemonIndex].currentStatus.TriggerStatusFeedback(fighterSpriteRenderer, coroutineHandler);
    }
    
    private void TriggerStatus()
    {
        pokemons[currentPokemonIndex].currentStatus.TriggerStatus();
        CheckPokemonIsDead();
    }

    public void RemoveStatus()
    {
        pokemons[currentPokemonIndex].currentStatus = null;
        statusBackground.gameObject.SetActive(false);
        statusText.gameObject.SetActive(false);
    }


 
}
