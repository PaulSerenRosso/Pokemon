using System;
[Serializable]
public abstract class PokemonCapacity
{
    private PokemonCapacitySO so;
    private int currentStamina;
    
    public virtual void Init(PokemonCapacitySO pokemonCapacitySo)
    {
        so = pokemonCapacitySo;
        currentStamina = so.maxStamina;
    }
    
    public float Stamina => currentStamina; 
    public virtual bool TryUseCapacity()
    {
        if (currentStamina == 0)
        {
            return false;
        }

        currentStamina--;
        return true;
    }

   

}
