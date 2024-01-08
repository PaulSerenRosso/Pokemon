using UnityEngine;

public class PlayerFighter : Fighter
{
   [SerializeField] private PokemonSO defaultPokemonSO;

   protected override void Start()
   {
       base.Start();
       pokemons.Add(defaultPokemonSO.CreatePokemon()); 
   }
}
