using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class DamageCapacity : PokemonCapacity
{
    private DamageCapacitySO damageCapacitySo;
   public override void Init(PokemonCapacitySO pokemonCapacitySo)
   {
      base.Init(pokemonCapacitySo);
      damageCapacitySo =(DamageCapacitySO) so;
   }

   public override bool TryUseCapacity(Pokemon pokemon, Pokemon enemyPokemon)
   {
       if (base.TryUseCapacity(pokemon, enemyPokemon))
       {
           enemyPokemon.DecreaseHp(damageCapacitySo.damage);
       }
       return false;
   }
}
