using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion", order = 3)]
public class PotionSO : ItemCommonSO
{
    public int healAmount = 10;
    public void HealPokemon(Pokemon pokemon)
    {
        pokemon.IncreaseHp(healAmount);
        DecrementItemCount();
    }
}
