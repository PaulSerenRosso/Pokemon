using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeStatus : PokemonStatus
{
    public FreezeStatus(Pokemon pokemon) : base(pokemon)
    {
        isTriggerAfterTurn = false;
        statusText = "FRZ";
        statusColor = new Color(0, 1, 1);
    }

    public override void TriggerStatus()
    {
        
    }

    public override bool CheckCanTriggerStatus()
    {
        if (Random.Range(0, 100) < 20)
        {
            isEndStatus = true;
            return false;
        }
        return true; 
    }

 
}
