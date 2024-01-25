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
    public bool isBackSprite;
    public Action chooseAnotherPokemonEvent;
    private PokemonStatusType currentStatusToAdd = PokemonStatusType.None;
    private Animator animator;
    private TextMeshProUGUI levelCountText;
    public Action refreshRendererEvent;
    protected virtual void Start()
    {
      
    }
    
    public virtual void Init(Fighter enemyFighter, SpriteRenderer fighterSpriteRenderer, SpriteRenderer enemySpriteRenderer, Slider hpSlider, TextMeshProUGUI nameText, TextMeshProUGUI statusText, Image statusBackground, MonoBehaviour coroutineHandler, Animator sceneAnimator, TextMeshProUGUI levelText, bool isBackSprite = false )
    {
        this.enemyFighter = enemyFighter;
        hasLost = false;
        currentPokemonIndex = 0;
       
        this.fighterSpriteRenderer = fighterSpriteRenderer;
        this.enemySpriteRenderer = enemySpriteRenderer;
        this.hpSlider = hpSlider;
        this.nameText = nameText;
        this.statusBackground = statusBackground;
        this.statusText = statusText;
        this.coroutineHandler = coroutineHandler;
        this.levelCountText = levelText;  
        this.isBackSprite = isBackSprite; 
          RefreshRenderer();
          animator = sceneAnimator;
          
    }

  

    public virtual void Disable()
    {
        disableEvent?.Invoke();
    }

    public bool TryEndTurn()
    {
        if (pokemons[currentPokemonIndex].IsDied)
        {
            Sequencer.Instance.AddCombatInteraction($"{GetCurrentPokemonName()} is K.O", true, () =>
            {
                if (CheckPokemonsAreAllDead())
                {
                    if(pokemons[currentPokemonIndex].currentStatus != null)
                        pokemons[currentPokemonIndex].currentStatus.RefreshTurnStateStatus();
                    endTurnEvent?.Invoke();
                    
                }
                else
                {
                    chooseAnotherPokemonEvent?.Invoke();   
                }
            });
            return true;
        }

        return false; 
    }
    
        public void EndOwnTurn()
    {
    
    
        enemyFighter.RefreshRenderer();
        RefreshRenderer();
        
        if (!enemyFighter.TryEndTurn())
        {   
            if (CheckTriggerEndTurnStatus())
            {   
                TriggerStatusFeedback();
            }
            else
            {
                if (pokemons[currentPokemonIndex].currentStatus != null)
                {    Debug.Log("test");
                    pokemons[currentPokemonIndex].currentStatus.RefreshTurnStateStatus();
                    endTurnEvent?.Invoke();
                }
                else
                {
                    if (currentStatusToAdd != PokemonStatusType.None)
                    {   
                        enemyFighter.AddStatus(currentStatusToAdd);
                        currentStatusToAdd = PokemonStatusType.None;
                    }
                    else
                    {
                        Debug.Log("test");
                        endTurnEvent?.Invoke();
                    }
                }
             
            }
        }

    }

    public string GetCurrentPokemonName()
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
        chooseActionEvent?.Invoke();
        Sequencer.Instance.AddCombatInteraction(
            $"{GetCurrentPokemonName()} uses {pokemons[currentPokemonIndex].capacities[index].so.name}", true, () =>
            {
                if (CheckUseCapacityStatus())
                {
                    TriggerStatusFeedback();
                }
                else
                {
                    pokemons[currentPokemonIndex].capacities[index].useCapacityFeedbackFinished = () => UseCapacity(index);
                    if (pokemons[currentPokemonIndex].capacities[index]
                        .TryUseCapacityFeedback(fighterSpriteRenderer, enemySpriteRenderer,  coroutineHandler,animator, isBackSprite ? "Player":"Enemy"))
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
            currentStatusToAdd = pokemons[currentPokemonIndex].capacities[index].pokemonStatusType;
           
        }
        EndOwnTurn();
    }

    public void RefreshRenderer()
    {
        levelCountText.text = pokemons[currentPokemonIndex].Level.ToString();
        nameText.text = pokemons[currentPokemonIndex].so.name;
        fighterSpriteRenderer.sprite = pokemons[currentPokemonIndex].so.battleSprite[isBackSprite ? 1:0];
        if (pokemons[currentPokemonIndex].currentStatus != null)
        {
                    statusBackground.gameObject.SetActive(true);
                    statusText.gameObject.SetActive(true);
                    statusText.text = pokemons[currentPokemonIndex].currentStatus.statusTextConcact;
                    statusBackground.color = pokemons[currentPokemonIndex].currentStatus.statusColor;
        }
        else
        {
            statusBackground.gameObject.SetActive(false);
        }
        hpSlider.value = (float)pokemons[currentPokemonIndex].Hp / pokemons[currentPokemonIndex].MaxHp;
        refreshRendererEvent?.Invoke();
    }

    public bool CheckTriggerEndTurnStatus()
    {
        if (pokemons[currentPokemonIndex].currentStatus != null &&  !pokemons[currentPokemonIndex].currentStatus.alreadyTriggerThisTurn)
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
            Debug.Log("check status condition");
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
        pokemons[currentPokemonIndex].currentStatus.useCapacityFeedbackFinished = AddUIStatus;
        pokemons[currentPokemonIndex].currentStatus.TriggerStatusFeedback(fighterSpriteRenderer, coroutineHandler);
    }

    private void AddUIStatus()
    {
        Sequencer.Instance.AddCombatInteraction($"{GetCurrentPokemonName()} is {pokemons[currentPokemonIndex].currentStatus.statusText}",true, ()=>
        {
            statusBackground.gameObject.SetActive(true);
            statusText.gameObject.SetActive(true);
            statusText.text = pokemons[currentPokemonIndex].currentStatus.statusTextConcact;
            statusBackground.color = pokemons[currentPokemonIndex].currentStatus.statusColor;
            endTurnEvent?.Invoke();
        });

    }

    private void TriggerStatusFeedback()
    {
        Debug.Log("bonsoir à tous et à toutes ");
        pokemons[currentPokemonIndex].currentStatus.useCapacityFeedbackFinished = TriggerStatus;
        pokemons[currentPokemonIndex].currentStatus.TriggerStatusFeedback(fighterSpriteRenderer, coroutineHandler);
    }
    
    private void TriggerStatus()
    {       
        Sequencer.Instance.AddCombatInteraction($"{GetCurrentPokemonName()} is {pokemons[currentPokemonIndex].currentStatus.statusText}",true, () =>
            {
                pokemons[currentPokemonIndex].currentStatus.TriggerStatus();
                EndOwnTurn();
            });
    }

    public void RemoveStatus()
    {
        pokemons[currentPokemonIndex].currentStatus = null;
        statusBackground.gameObject.SetActive(false);
        statusText.gameObject.SetActive(false);
    }


 
}
