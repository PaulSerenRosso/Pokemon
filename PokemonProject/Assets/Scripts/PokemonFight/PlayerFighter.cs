using UnityEngine;

public class PlayerFighter : Fighter
{
   [SerializeField] private PokemonSO[] defaultPokemonsSO;

   protected override void Start()
   {
       base.Start();
       foreach (var pokemon in defaultPokemonsSO)
       {
           pokemons.Add(pokemon.CreatePokemon()); 
       }
   }
   public void AddPokemon(PokemonSO pokemonSo)
   {
       pokemons.Add(pokemonSo.CreatePokemon());
   }
}
