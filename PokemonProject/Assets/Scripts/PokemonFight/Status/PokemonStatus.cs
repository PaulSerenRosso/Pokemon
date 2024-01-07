using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class PokemonStatus
{
    public bool isTriggerAfterTurn;
    private Pokemon pokemon;
    public Action useCapacityFeedbackFinished;
    public string statusText;
    public Color statusColor;
    public PokemonStatus(Pokemon pokemon)
    {
        this.pokemon = pokemon;
    }
    public abstract void TriggerStatus();

    public virtual void TriggerStatusFeedback(SpriteRenderer spriteRenderer, MonoBehaviour coroutineHandler)
    {
        coroutineHandler.StartCoroutine(WaitForEndFeedback());
    }
    
    IEnumerator WaitForEndFeedback()
    {
        yield return new WaitForSeconds(1f);
        useCapacityFeedbackFinished?.Invoke();
    }
}
