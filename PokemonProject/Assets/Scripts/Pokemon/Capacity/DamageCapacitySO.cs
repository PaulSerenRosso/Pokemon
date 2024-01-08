using System;
using UnityEngine;
[CreateAssetMenu(fileName = "DamageCapacitySO", menuName = "Pokemon/DamageCapacitySO", order = 1)]
public class DamageCapacitySO : PokemonCapacitySO
{
    public int damage;
    public override Type GetType()
    {
        return typeof(DamageCapacity);
    }
}
