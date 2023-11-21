using System;
using UnityEngine;

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
    
    protected virtual void Start()
    {
        pokemons = new Pokemon[6];
    }

    [SerializeField] 
    public virtual void Init(Fighter enemyFighter)
    {
        this.enemyFighter = enemyFighter;
        hasLost = false;
        currentPokemonIndex = 0;
        initEvent?.Invoke();

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
    
    

  
}
