using System;
using UnityEngine;

[Serializable]
public abstract class PokemonCapacity
{
    protected PokemonCapacitySO so;
    private int currentStamina;
    public Action useCapacityFeedbackFinished;
    
    public virtual void Init(PokemonCapacitySO pokemonCapacitySo)
    {
        so = pokemonCapacitySo;
        currentStamina = so.maxStamina;
    }
    
    public float Stamina => currentStamina; 
    public virtual bool TryUseCapacityFeedback(SpriteRenderer pokemon, SpriteRenderer enemyPokemon)
    {
        if (currentStamina == 0)
        {
            return false;
        }
       
        useCapacityFeedbackFinished?.Invoke();
        return true;
    }
    
    public virtual bool TryUseCapacity(Pokemon pokemon, Pokemon enemyPokemon)
    {
        currentStamina--;
        return true;
    }
    
    

   

}
