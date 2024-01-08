using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonTrainer : MonoBehaviour
{
    [SerializeField] private EnemyFighter enemyFighter;
    public PokemonSO  pokemonSo;
    private PlayerManager playerManager;

    public void TriggerFight()
    {
        enemyFighter.pokemons.Add(pokemonSo.CreatePokemon());
        playerManager.fightManager.InitFight(enemyFighter);
    }
}
