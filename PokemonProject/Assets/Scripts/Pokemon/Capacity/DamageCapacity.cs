using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class DamageCapacity : PokemonCapacity
{
   public override void Init(PokemonCapacitySO pokemonCapacitySo)
   {
      base.Init(pokemonCapacitySo);
   }

   public override bool TryUseCapacity()
   {
      return base.TryUseCapacity();
   }
}
