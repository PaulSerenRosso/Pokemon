using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ParalysisStatus : PokemonStatus
{
    public ParalysisStatus(Pokemon pokemon) : base(pokemon)
    {
        isTriggerAfterTurn = false;
        statusColor = new Color(1, 1, 0);
        statusTextConcact = "PAR";
        captureFactor = 1.5f;
        statusText = "paralysed";
    }

    public override void TriggerStatus()
    {
        base.TriggerStatus();
    }

    public override bool CheckCanTriggerStatus()
    {
        var rand = Random.Range(0, 100);
        if (rand < 25)
        {
            return true;
        }
        return false; 
    }

 
}
