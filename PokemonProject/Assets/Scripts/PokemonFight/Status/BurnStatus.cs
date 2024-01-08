using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnStatus : PokemonStatus
{
    public BurnStatus(Pokemon pokemon) : base(pokemon)
    {
        base.isTriggerAfterTurn = true;
        statusColor = new Color(1, 0, 0);
        statusText = "BRN";
    }

    public override void TriggerStatus()
    {
        pokemon.DecreaseHp(pokemon.MaxHp/8);
    }

    public override bool CheckCanTriggerStatus()
    {
        return true;
    }
}
