using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

   public override void Init(Fighter enemyFighter, SpriteRenderer fighterSpriteRenderer, SpriteRenderer enemySpriteRenderer,
       Slider hpSlider, TextMeshProUGUI nameText, TextMeshProUGUI statusText, Image statusBackground,
       MonoBehaviour coroutineHandler, Animator sceneAnimator, TextMeshProUGUI levelText, bool isBackSprite)
   {
       base.Init(enemyFighter, fighterSpriteRenderer, enemySpriteRenderer, hpSlider, nameText, statusText, statusBackground, coroutineHandler,sceneAnimator, levelText, isBackSprite);
      
   }
}
