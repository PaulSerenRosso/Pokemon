using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class PokemonStatus
{
    public bool isTriggerAfterTurn;
    protected Pokemon pokemon;
    public Action useCapacityFeedbackFinished;
    public string statusTextConcact;
    public Color statusColor;
    public bool isEndStatus;
    public float captureFactor;
    public string statusText;
    public bool alreadyTriggerThisTurn = false;
    public PokemonStatus(Pokemon pokemon)
    {
        this.pokemon = pokemon;
    }

    public void RefreshTurnStateStatus()
    {
        alreadyTriggerThisTurn = false;
    }

    public virtual void TriggerStatus()
    {
        alreadyTriggerThisTurn = true;
    }

    public abstract bool CheckCanTriggerStatus();

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
