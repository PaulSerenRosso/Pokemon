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
    [SerializeField] private GameObject pokemonPanel;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject menuHelper;
   
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
        menuHelper.SetActive(menuIsActive);
        startMenu.SetActive(menuIsActive);
        pokemonPanel.SetActive(false);
    }

    private void OnDisable()
    {
        playerController.playerInputs.UI.OpenCloseMenu.performed -= OpenCloseMenu;
    }

    public void ActivatePokemonPanel()
    {
        Debug.Log("test");
        menuHelper.SetActive(false);
        startMenu.SetActive(false);
        pokemonPanel.SetActive(true);
    }

   



}
