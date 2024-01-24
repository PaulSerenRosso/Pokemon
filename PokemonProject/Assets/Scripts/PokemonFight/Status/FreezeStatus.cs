using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeStatus : PokemonStatus
{
    public FreezeStatus(Pokemon pokemon) : base(pokemon)
    {
        isTriggerAfterTurn = false;
        statusTextConcact = "FRZ";
        statusColor = new Color(0, 1, 1);   
        captureFactor = 2.5f;
        statusText = "frozen";
    }

    public override void TriggerStatus()
    {
        base.TriggerStatus();
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
