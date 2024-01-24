using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "KeyItem", menuName = "Items/Pokeball", order = 2)]
public class PokeballItemSO : ItemSO
{
    public float captureFactor;
    public Action useBeginCinematicFeedback;
    public Action useEndCinematicFeedback;
    
    public bool UsePokeball(Pokemon pokemon)
    {  var ratioHP = (float)pokemon.Hp / pokemon.MaxHp;
        var statusFactor = pokemon.currentStatus == null ? 1 : pokemon.currentStatus.captureFactor;
        var captureRate = (1 - 2/3 * ratioHP)*statusFactor * captureFactor * pokemon.so.captureFactor;
        if (Random.Range(0,255) <= captureRate)
        {
            return true;
        }
        return false;
    }

    public void UsePokemonBeginCinematicFeedback(MonoBehaviour coroutineHandler, SpriteRenderer spriteRenderer)
    {
        coroutineHandler.StartCoroutine(WaitForBeginCinematicFeedback(spriteRenderer));
    }


    
    IEnumerator WaitForBeginCinematicFeedback(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(1f);
        useBeginCinematicFeedback?.Invoke();
    }

    public void UsePokeballEndCinematicFeedback(MonoBehaviour coroutineHandler, SpriteRenderer spriteRenderer, bool isSucceed)
    {
        coroutineHandler.StartCoroutine(WaitForEndCinematicFeedback(spriteRenderer, isSucceed));
    }
    IEnumerator WaitForEndCinematicFeedback(SpriteRenderer spriteRenderer, bool isSucceed)
    {
        yield return new WaitForSeconds(1f);
        useEndCinematicFeedback?.Invoke();
    }


    
}
