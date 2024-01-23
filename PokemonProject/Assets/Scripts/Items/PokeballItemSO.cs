using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "Items/Pokeball", order = 2)]
public class PokeballItemSO : ItemSO
{
    
    public Action useCapacityFeedbackFinished;
    public int numberOfItems = 1;
    public void IncrementItemCount()
    {
        numberOfItems++;
    }

    public void UsePokeball(Pokemon pokemon)
    {
        
    }

    public void UsePokeballFeedback(MonoBehaviour coroutineHandler, SpriteRenderer spriteRenderer)
    {
        coroutineHandler.StartCoroutine(WaitForEndFeedback(spriteRenderer));
    }
    
    IEnumerator WaitForEndFeedback(SpriteRenderer spriteRenderer)
    {
        yield return new WaitForSeconds(1f);
        useCapacityFeedbackFinished?.Invoke();
    }

    public void DecrementItemCount()
    {
        numberOfItems = Mathf.Max(0, numberOfItems - 1);
    }
    
}
