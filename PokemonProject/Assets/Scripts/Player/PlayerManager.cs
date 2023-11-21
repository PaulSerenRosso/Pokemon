using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerFighter playerFighter;
    public FightManager fightManager;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        playerFighter.initEvent += () =>
        {
            playerController.SetCameraActive(false);
            playerController.enabled = false;
        };

        playerFighter.disableEvent += () =>
        {
            playerController.SetCameraActive(true);
            playerController.enabled = true;
        };
    }
    
    
}
