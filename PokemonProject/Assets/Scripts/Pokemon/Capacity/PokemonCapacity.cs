using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public abstract class PokemonCapacity
{
    public PokemonCapacitySO so;
    private int currentStamina;
    public Action useCapacityFeedbackFinished;
    public PokemonStatusType pokemonStatusType;
    
    public virtual void Init(PokemonCapacitySO pokemonCapacitySo)
    {
     
        so = pokemonCapacitySo;
        pokemonStatusType = so.pokemonStatusType;
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

    public bool TryUseStatus()
    {
        if (Random.Range(0, 100) <= so.probabilityOfInflictStatus)
        {
            return true;
        }
        return false;
    }
    
    

   

}
