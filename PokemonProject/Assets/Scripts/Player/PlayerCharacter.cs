using SequencerNS;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : Mover
{
    [SerializeField] private FightManager fightManager;
    [SerializeField] private Sequencer sequencer;
    
    
    //is not moving, launch raycast interagit
    // onIteract dialogue
    
    // si je suis en interaction je lance le raycast et je lance une fonction sur le sequencer
    
    #region Interact
    public void InteractInput(InputAction.CallbackContext ctx)
    {
        //if (fightManager.isInFight) return;
        
        if (ctx.started)
        {
            if (sequencer.CurrentSequenceType == SequenceType.None && !IsMoving) // Raycast pour savoir y'a quoi
            {
                var go = GetObjectForward();
                go?.GetComponent<IInteractable>()?.Interact();
                Debug.Log("TryGetForwardObjForDialogue");
            }
            else
            {
                sequencer.OnClick();
            }
        }

        if (ctx.performed)
        {
            sequencer.OnPerformedClick();
        }

        if (ctx.canceled)
        {
            sequencer.OnReleaseClick();
        }
    }
    #endregion
}
