using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SleepStatus : PokemonStatus
{
    private int sleepCount;
    public SleepStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = false;
        sleepCount = Random.Range(1, 6);
        statusColor = new Color(0.5f, 0.5f, 0.5f);
        statusTextConcact = "SLP";
        captureFactor = 2.5f;
        statusText = "slept";
    }

    public override void TriggerStatus()
    {
        base.TriggerStatus();
        sleepCount--;
    }

    public override bool CheckCanTriggerStatus()
    {
        Debug.Log(sleepCount);
        if (sleepCount <= 0)
        {
            isEndStatus = true;
            return false; 
        }

        return true;
    }
    
}
