using System.Collections.Generic;
using SequencerNS;
using UnityEngine;

public class DialogueObjects : MonoBehaviour, IInteractable
{
    public ObjectType objectType;
    public List<InteractionSO> interactionsToSend = new(); 
    public List<InteractionSO> interactionsOneShot = new();

    
    [Header("IF PNG")]
    public SpriteRenderer sR;
    public List<Sprite> pngSprites = new(); // 1 - Side, North, South
    
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
            for (int i = 0; i < interactionsOneShot.Count; i++)
            {
                Sequencer.Instance.AddPopInteraction(interactionsOneShot[i]);
            }
            
            interactionsOneShot.Clear();
            return;
        }
        
        if (interactionsToSend.Count != 0)
        {
            Sequencer.Instance.AddPopInteraction(interactionsToSend[0]);
        }
    }

    public void SetGoodDirSprite()
    {
        var dir = Mover.LookDirection(PlayerManager.Instance.transform.position, transform.position);
        Debug.Log(dir.ToString());
        switch (dir)
        {
            case Direction.East: sR.sprite = pngSprites[0]; sR.flipX = true; break;
            case Direction.West: sR.sprite = pngSprites[0]; sR.flipX = false; break;
            case Direction.North: sR.flipX = false; sR.sprite = pngSprites[1]; break;
            case Direction.South: sR.flipX = false; sR.sprite = pngSprites[2]; break;
        }
    }
}

public enum ObjectType
{
    WorldObject,
    StaticNPC
}
 