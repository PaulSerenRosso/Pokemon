using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ParalysisStatus : PokemonStatus
{
    public ParalysisStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = false;
        statusColor = new Color(1, 1, 0);
        statusText = "PAR";
    }

    public override void TriggerStatus()
    {
        
    }

    public override bool CheckCanTriggerStatus()
    {
        if (Random.Range(0, 100) < 25)
        {
            return true;
        }
        return false; 
    }

 
}
