using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HighGrass : MonoBehaviour
{
    [SerializeField] private EnemyFighter enemyFighter;
    [SerializeField] private PokemonSO[] allPokemonSo;
    [SerializeField] private float probabilityToEngageFight;
    private PlayerCharacter playerCharacter;
    private PlayerManager playerManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var mover = other.GetComponent<PlayerCharacter>();
        playerManager = other.GetComponent<PlayerManager>();
        playerCharacter = mover;
        if (!playerManager.playerFighter.CheckLose())
        {
              mover.endMoveEvent += TryTriggerFight;
        }
      
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (playerCharacter == other.GetComponent<PlayerCharacter>())
        {
            playerCharacter.endMoveEvent -= TryTriggerFight;
        }
    }

    private void TryTriggerFight(Mover mover)
    {
        float engageFightDraw = Random.Range(0.0f, 1.0f);
        if (engageFightDraw <= probabilityToEngageFight)
        {
            int randomPokemonIndex = Random.Range(0, allPokemonSo.Length);
            enemyFighter.pokemons.Add(allPokemonSo[randomPokemonIndex].CreatePokemon());
             enemyFighter.canCapturedPokemons = true;
             enemyFighter.pokemonAdjective = " wild";
             playerManager.fightManager.InitFight(enemyFighter);
           
        }
    }
}
