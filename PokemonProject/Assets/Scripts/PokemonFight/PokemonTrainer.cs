using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonTrainer : MonoBehaviour
{
    [SerializeField] private EnemyFighter enemyFighter;
    [SerializeField] private PokemonSO pokemonSo;
    private PlayerManager playerManager;

    public void TriggerFight()
    {
        enemyFighter.pokemons.Add(pokemonSo.CreatePokemon());
        playerManager.fightManager.InitFight(enemyFighter);
    }
}
