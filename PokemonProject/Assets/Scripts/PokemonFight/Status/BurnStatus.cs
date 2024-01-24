using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnStatus : PokemonStatus
{
    public BurnStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = true;
        statusColor = new Color(1, 0, 0);
        statusTextConcact = "BRN";
        captureFactor = 1.5f;
        statusText = "burnt";
    }

    public override void TriggerStatus()
    {  base.TriggerStatus();
        pokemon.DecreaseHp(pokemon.MaxHp/8);
    }

    public override bool CheckCanTriggerStatus()
    {
        return true;
    }
}
