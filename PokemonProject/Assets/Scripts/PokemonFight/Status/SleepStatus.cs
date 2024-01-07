using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SleepStatus : PokemonStatus
{
    public SleepStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = false;
    }

    public override void TriggerStatus()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerStatusFeedback([CanBeNull] SpriteRenderer spriteRenderer , MonoBehaviour coroutineHandler)
    {
        throw new System.NotImplementedException();
    }
}
