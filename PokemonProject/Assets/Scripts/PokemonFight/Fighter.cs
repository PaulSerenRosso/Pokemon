using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    public Pokemon[] pokemons;
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
    protected virtual void Start()
    {
        pokemons = new Pokemon[6];
    }
    
    public virtual void Init(Fighter enemyFighter, SpriteRenderer fighterSpriteRenderer, SpriteRenderer enemySpriteRenderer, Slider hpSlider, TextMeshProUGUI nameText )
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
        this.fighterSpriteRenderer.sprite = pokemons[currentPokemonIndex].so.sprite;
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
        foreach (var pokemon in pokemons)
        {
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
        if (pokemons[currentPokemonIndex].capacities[index]
            .TryUseCapacityFeedback(fighterSpriteRenderer, enemySpriteRenderer))
        {
            pokemons[currentPokemonIndex].capacities[index].useCapacityFeedbackFinished = () => UseCapacity(index);
            chooseActionEvent?.Invoke();
        };
    }
    
    public void UseCapacity(int index)
    {
        pokemons[currentPokemonIndex].capacities[index].TryUseCapacity(pokemons[currentPokemonIndex], enemyFighter.pokemons[enemyFighter.currentPokemonIndex]);
        endTurnEvent?.Invoke();
    }

    public void RefreshRenderer()
    {
        hpSlider.value = (float)pokemons[currentPokemonIndex].Hp / pokemons[currentPokemonIndex].MaxHp;
    }
    
    

  
}
