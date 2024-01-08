using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance => instance;
    
    public PlayerFighter playerFighter;
    public FightManager fightManager;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
