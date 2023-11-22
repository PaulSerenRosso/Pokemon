using System;
using System.Collections;
using System.Collections.Generic;
using SequencerNS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerController : MonoBehaviour
{
    public PlayerInputs playerInputs = null;
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private Sequencer sequencer;
    [SerializeField] private Camera characterCamera;
    [SerializeField] private MenuManager _menuManager;
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Enable();
        _menuManager.EnableInput();
    }

    public void SetCameraActive(bool value)
    {
        characterCamera.gameObject.SetActive(value);
        
    }

    private void OnEnable()
    {
        playerInputs.Enable();
        playerInputs.Player.MoveLeft.performed += MoveLeft;
        playerInputs.Player.MoveRight.performed += MoveRight;
        playerInputs.Player.MoveUp.performed += MoveUp;
        playerInputs.Player.MoveDown.performed += MoveDown;
        playerInputs.Player.MoveUp.canceled += CancelMoveUp;
        playerInputs.Player.MoveLeft.canceled += CancelMoveLeft;
        playerInputs.Player.MoveRight.canceled += CancelMoveRight;
        playerInputs.Player.MoveDown.canceled += CancelMoveDown;
       
        // playerInputs.Player.Interact.started += OnInteract;
        // playerInputs.Player.Interact.canceled += OnReleaseInteract;
        // playerInputs.Player.Interact.performed += OnPerformedInteract;
    }

    private void CancelMoveUp(InputAction.CallbackContext obj)
    {
        playerCharacter.RemoveDirection(Vector2.up);
    }
    
    private void CancelMoveDown(InputAction.CallbackContext obj)
    {
        playerCharacter.RemoveDirection(Vector2.down);
    }
    
    private void CancelMoveLeft(InputAction.CallbackContext obj)
    {
        playerCharacter.RemoveDirection(Vector2.left);
    }
    
    
    private void CancelMoveRight(InputAction.CallbackContext obj)
    {
        playerCharacter.RemoveDirection(Vector2.right);
    }

    private void MoveDown(InputAction.CallbackContext obj)
    {
        if (sequencer.CurrentSequenceType == SequenceType.None)
            playerCharacter.AddDirection(Vector2.down);
    }

    private void MoveUp(InputAction.CallbackContext obj)
    {
        if (sequencer.CurrentSequenceType == SequenceType.None)
            playerCharacter.AddDirection(Vector2.up);
    }

    private void MoveRight(InputAction.CallbackContext obj)
    {
        if (sequencer.CurrentSequenceType == SequenceType.None)
            playerCharacter.AddDirection(Vector2.right);
    }

    private void MoveLeft(InputAction.CallbackContext obj)
    {
        if (sequencer.CurrentSequenceType == SequenceType.None)
            playerCharacter.AddDirection(Vector2.left);
    }
    
    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.MoveLeft.performed -= MoveLeft;
        playerInputs.Player.MoveRight.performed -= MoveRight;
        playerInputs.Player.MoveUp.performed -= MoveUp;
        playerInputs.Player.MoveDown.performed -= MoveDown;
        playerInputs.Player.MoveUp.canceled -= CancelMoveUp;
        playerInputs.Player.MoveLeft.canceled -= CancelMoveLeft;
        playerInputs.Player.MoveRight.canceled -= CancelMoveRight;
        playerInputs.Player.MoveDown.canceled -= CancelMoveDown;
    }
}
