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
        Debug.Log(mover);
        playerManager = other.GetComponent<PlayerManager>();
        playerCharacter = mover;
        mover.endMoveEvent += TryTriggerFight;
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
            enemyFighter.pokemons[0] = allPokemonSo[randomPokemonIndex].CreatePokemon();
            playerManager.fightManager.InitFight(enemyFighter);
        }
    }
}
