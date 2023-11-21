using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    [SerializeField] private PlayerFighterController playerFighterController;
    [SerializeField] private EnemyFighterController enemyFighterController;
    private bool isPlayerTurn;
    private GameObject previousSpace;
    [SerializeField] private GameObject fightSpace;
    
    private void Start()
    {
        playerFighterController.endTurnEvent = ChangeTurn;
        enemyFighterController.endTurnEvent = ChangeTurn;
    }

    public void InitFight(Fighter enemyFighter)
    {
        previousSpace = WorldManager.instance.CurrentSpace;
        WorldManager.instance.ChangeSpace(fightSpace);
        enemyFighterController.fighter = enemyFighter;
        playerFighterController.fighter.Init(enemyFighter);
        enemyFighterController.fighter.Init(playerFighterController.fighter);
        isPlayerTurn = true;
        ChangeTurn();
    }

    public void ChangeTurn()
    {
        if (enemyFighterController.fighter.CheckLose())
        {
            EndFight();
            return;
        }
        if(playerFighterController.fighter.CheckLose())
        {
            EndFight();
            return;
        }
        isPlayerTurn = !isPlayerTurn;
        if (isPlayerTurn)
        {
            playerFighterController.Activate();
            enemyFighterController.Deactivate();
        }
        else
        {
            playerFighterController.Deactivate();
            enemyFighterController.Activate();
        }
    }
    
    
    void EndFight()
    {
        WorldManager.instance.ChangeSpace(previousSpace);
    }
}
