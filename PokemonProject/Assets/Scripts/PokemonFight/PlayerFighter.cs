using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : Fighter
{
   [SerializeField] private PokemonSO defaultPokemonSO;
   protected override void Start()
   {
       base.Start();
       pokemons[0] = defaultPokemonSO.CreatePokemon();
   }

   public override void Init(Fighter enemyFighter)
   {
       base.Init(enemyFighter);
     
   }
}
