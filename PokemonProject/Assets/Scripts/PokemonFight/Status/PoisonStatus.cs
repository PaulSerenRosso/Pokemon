using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatus : PokemonStatus
{
    public PoisonStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = true;
    }

    public override void TriggerStatus()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerStatusFeedback(SpriteRenderer spriteRenderer, MonoBehaviour coroutineHandler)
    {
   
    }
    

}
