using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeStatus : PokemonStatus
{
    public FreezeStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = false;
    }

    public override void TriggerStatus()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerStatusFeedback(SpriteRenderer spriteRenderer, MonoBehaviour coroutineHandler)
    {
        throw new System.NotImplementedException();
    }
}
