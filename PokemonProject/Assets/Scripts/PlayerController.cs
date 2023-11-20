using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private PlayerInputs playerInputs = null;
    [SerializeField] private PlayerCharacter playerCharacter;

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
        playerInputs.Player.MoveUp.canceled += CancelMove;
        playerInputs.Player.MoveLeft.canceled += CancelMove;
        playerInputs.Player.MoveRight.canceled += CancelMove;
        playerInputs.Player.MoveDown.canceled += CancelMove;
    }

    private void CancelMove(InputAction.CallbackContext obj)
    {
        playerCharacter.SetMoveDirection(Vector2.zero);
    }

    private void MoveDown(InputAction.CallbackContext obj)
    {
        playerCharacter.SetMoveDirection(Vector2.down);
    }

    private void MoveUp(InputAction.CallbackContext obj)
    {
       
        playerCharacter.SetMoveDirection(Vector2.up);
    }

    private void MoveRight(InputAction.CallbackContext obj)
    {
       
        playerCharacter.SetMoveDirection(Vector2.right);
    }

    private void MoveLeft(InputAction.CallbackContext obj)
    {
      
        playerCharacter.SetMoveDirection(Vector2.left);
    }

    private void OnDisable()
    {
        playerInputs.Disable();
        playerInputs.Player.MoveLeft.performed -= MoveLeft;
        playerInputs.Player.MoveRight.performed -= MoveRight;
        playerInputs.Player.MoveUp.performed -= MoveUp;
        playerInputs.Player.MoveDown.performed -= MoveDown;
        playerInputs.Player.MoveUp.canceled -= CancelMove;
        playerInputs.Player.MoveLeft.canceled -= CancelMove;
        playerInputs.Player.MoveRight.canceled -= CancelMove;
        playerInputs.Player.MoveDown.canceled -= CancelMove;
    }

 
}
