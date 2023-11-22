
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    [SerializeField] private PlayerFighterController playerFighterController;
    [SerializeField] private EnemyFighterController enemyFighterController;
    private bool isPlayerTurn;
    private GameObject previousSpace;
    [SerializeField] private GameObject fightSpace;
    // UI 
    [SerializeField] private TextMeshProUGUI playerPokemonTextName;
    [SerializeField] private TextMeshProUGUI enemyPokemonTextName;
    [SerializeField] private Slider playerPokemonSlider;
    [SerializeField] private Slider enemyPokemonSlider;
    public bool isInFight;
    
    private void Start()
    {
        playerFighterController.fighter.chooseActionEvent = ResolveFight;
        playerFighterController.fighter.endTurnEvent = ChangeTurn;
    }

    public void InitFight(Fighter enemyFighter)
    {
        GetComponent<Camera>().gameObject.SetActive(true);
        previousSpace = WorldManager.instance.CurrentSpace;
        WorldManager.instance.ChangeSpace(fightSpace);
        enemyFighterController.fighter = enemyFighter;
        enemyFighterController.fighter.chooseActionEvent =ResolveFight;
        enemyFighterController.fighter.endTurnEvent = ChangeTurn;
        playerFighterController.fighter.Init(enemyFighter);
        enemyFighterController.fighter.Init(playerFighterController.fighter);
        isPlayerTurn = true;
        isInFight = true;
        ChangeTurn();
    }

    void ResolveFight()
    {
        playerFighterController.Deactivate();
        enemyFighterController.Deactivate();
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
        
        }
        else
        {
            enemyFighterController.Activate();
        }
    }
    
    void EndFight()
    {
        isInFight = false;
        WorldManager.instance.ChangeSpace(previousSpace);
    }
}
