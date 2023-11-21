using System;
using UnityEngine;

public abstract class PokemonCapacitySO : ScriptableObject
{
    public string name;
    public int maxStamina;
    public PokemonTypeSO pokemonTypeSo;
    public abstract Type GetType();

}