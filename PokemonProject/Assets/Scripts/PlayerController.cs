using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private PlayerInputs playerInputs = null;
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PopUpManager popUpManager;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
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
        playerInputs.Player.Interact.started += OnInteract;
        playerInputs.Player.Interact.canceled += OnReleaseInteract;
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
        playerCharacter.AddDirection(Vector2.down);
    }

    private void MoveUp(InputAction.CallbackContext obj)
    {
       
        playerCharacter.AddDirection(Vector2.up);
    }

    private void MoveRight(InputAction.CallbackContext obj)
    {
       
        playerCharacter.AddDirection(Vector2.right);
    }

    private void MoveLeft(InputAction.CallbackContext obj)
    {
      
        playerCharacter.AddDirection(Vector2.left);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        popUpManager.OnClick();
    }

    private void OnReleaseInteract(InputAction.CallbackContext ctx)
    {
        popUpManager.OnReleaseClick();
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
