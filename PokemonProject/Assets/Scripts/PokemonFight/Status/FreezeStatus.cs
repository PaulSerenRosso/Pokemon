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
        captureFactor = 2.5f;
    }

    public override void TriggerStatus()
    {
        
    }

    public override bool CheckCanTriggerStatus()
    {
        var rand = Random.Range(0, 100);
        if (rand < 20)
        {
            isEndStatus = true;
            return false;
        }
        return true; 
    }

 
}
