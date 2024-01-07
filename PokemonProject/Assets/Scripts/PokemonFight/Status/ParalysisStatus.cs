using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ParalysisStatus : PokemonStatus
{
    public ParalysisStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = false;
    }

    public override void TriggerStatus()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerStatusFeedback( SpriteRenderer spriteRenderer, MonoBehaviour coroutineHandler)
    {
        throw new System.NotImplementedException();
    }
}
