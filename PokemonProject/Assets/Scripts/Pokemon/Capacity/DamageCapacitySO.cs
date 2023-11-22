using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageCapacitySO", menuName = "Pokemon/DamageCapacitySO", order = 1)]
public class DamageCapacitySO : PokemonCapacitySO
{
    public override Type GetType()
    {
        return typeof(DamageCapacitySO);
    }
}
