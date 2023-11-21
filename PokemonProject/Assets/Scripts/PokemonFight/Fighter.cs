using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected Pokemon[] pokemons;
    private Fighter enemyFighter;
    private bool hasLost;
    public virtual void Init(Fighter enemyFighter)
    {
        this.enemyFighter = enemyFighter;
        hasLost = false; 
        
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
