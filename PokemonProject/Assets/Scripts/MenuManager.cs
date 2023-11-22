using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject menuPanel;
    private bool menuIsActive = false;
    private void OnEnable()
    {
        if(playerController.playerInputs != null)
        playerController.playerInputs.UI.OpenCloseMenu.performed += OpenCloseMenu;
    }
    public void EnableInput()
    {
        playerController.playerInputs.UI.OpenCloseMenu.performed += OpenCloseMenu;
    }
    private void OpenCloseMenu(InputAction.CallbackContext obj)
    {
        menuIsActive = !menuIsActive;
        menuPanel.SetActive(menuIsActive);
    }

    private void OnDisable()
    {
        playerController.playerInputs.UI.OpenCloseMenu.performed -= OpenCloseMenu;
    }

   



}
