using UnityEngine;
[CreateAssetMenu(fileName = "PokemonSO", menuName = "Pokemon/PokemonSO", order = 0)]
public class PokemonSO : ScriptableObject
{
    public string name;
    public int startLevel;
    public PokemonCapacitySO[] pokemonCapacitySO;
    public int maxHp;
    public PokemonTypeSO pokemonTypeSo;

    public Pokemon CreatePokemon()
    {
        return new Pokemon(this);
    }

  
}
