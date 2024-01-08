using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatus : PokemonStatus
{
    public PoisonStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = true;
        statusColor = new Color(1, 0, 1);
        statusText = "PSN";
    }

    public override void TriggerStatus()
    {
        pokemon.DecreaseHp(pokemon.MaxHp/8);
    }

    public override bool CheckCanTriggerStatus()
    {
        return true;
    }
    
    

}
