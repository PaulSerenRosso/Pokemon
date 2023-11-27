using System.Collections.Generic;
using SequencerNS;
using UnityEngine;

public class DialogueObjects : Mover, IInteractable
{
    [Space(3)]
    [Header("DialogueObjects Section")]
    public ObjectType objectType;
    public List<InteractionSO> interactionsToSend = new(); 
    public List<InteractionSO> interactionsOneShot = new();
    
    
    private bool isInteractable = true;
    public bool IsInteractable => isInteractable;

    bool IInteractable.IsInteractable() => IsInteractable;

    public void Interact()
    {
        if (objectType == ObjectType.StaticNPC)
        {
            SetGoodDirSprite();
        }
        
        if (interactionsOneShot.Count != 0)
        {
            Sequencer.Instance.AddPopInteraction(interactionsOneShot[0]);
            interactionsOneShot.RemoveAt(0);
            return;
        }
        
        if (interactionsToSend.Count != 0)
        {
            Sequencer.Instance.AddPopInteraction(interactionsToSend[0]);
        }
    }

    private void SetGoodDirSprite()
    {
        var dir = LookDirection(PlayerManager.Instance.transform.position, transform.position);
        Debug.Log(dir.ToString());
        switch (dir)
        {
            case Direction.East: StartCoroutine(Rotate(new Vector2(1, 0))); break;
            case Direction.West: StartCoroutine(Rotate(new Vector2(-1, 0))); break;
            case Direction.North: StartCoroutine(Rotate(new Vector2(0, 1))); break;
            case Direction.South: StartCoroutine(Rotate(new Vector2(0, -1))); break;
        }
    }
}

public enum ObjectType
{
    WorldObject,
    StaticNPC
}
 