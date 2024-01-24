using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private bool menuIsActive = false;
    [SerializeField] private GameObject pokemonPanel;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject menuHelper;
    [SerializeField] private GameObject bagPanel;
    [SerializeField] private FightManager fightManager; 
   
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
        if (fightManager.isInFight)
        {
            return;
        }
        menuIsActive = !menuIsActive;
        menuHelper.SetActive(menuIsActive);
        startMenu.SetActive(menuIsActive);
        pokemonPanel.SetActive(false);
    }

    private void OnDisable()
    {
        playerController.playerInputs.UI.OpenCloseMenu.performed -= OpenCloseMenu;
    }
    
    public void ActivateBagPanel()
    {
        menuHelper.SetActive(false);
        startMenu.SetActive(false);
        bagPanel.SetActive(true);
    }

    public void ActivatePokemonPanel()
    {
        Debug.Log("test");
        menuHelper.SetActive(false);
        startMenu.SetActive(false);
        pokemonPanel.SetActive(true);
    }
    
    public void DeactivatePokemonPanel()
    {
        if (!fightManager.isInFight)
        {
            menuHelper.SetActive(true);
            startMenu.SetActive(true);
        }
        pokemonPanel.SetActive(false);
    }

    public void CloseMenuHelper()
    {
        menuHelper.SetActive(false);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
