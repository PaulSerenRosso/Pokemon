using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFighterController : FighterController
{
    public override void Activate()
    {
        int randomCapacityIndex = Random.Range(0, fighter.pokemons[fighter.currentPokemonIndex].GetActiveCapacityCount());
        fighter.UseCapacityFeedback( randomCapacityIndex, this);
    }
    public override void Deactivate()
    {
       
    }
}
