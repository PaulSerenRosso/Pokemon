using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonTypeSO", menuName = "Pokemon/PokemonTypeSO", order = 2)]
public class PokemonTypeSO : ScriptableObject
{
    public Color color;
    public string name; 
}
