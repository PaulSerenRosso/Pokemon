using System;
using System.Collections;
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
    public virtual bool TryUseCapacityFeedback(SpriteRenderer pokemon, SpriteRenderer enemyPokemon, MonoBehaviour coroutineHandler)
    {
        if (currentStamina == 0)
        {
            return false;
        }
        coroutineHandler.StartCoroutine(WaitForEndFeedback());
        return true;
    }

    IEnumerator WaitForEndFeedback()
    {
        yield return new WaitForSeconds(1f);
        useCapacityFeedbackFinished?.Invoke();
    }
    
    public virtual bool TryUseCapacity(Pokemon pokemon, Pokemon enemyPokemon)
    {
        currentStamina--;
        return true;
    }
    
    

   

}
