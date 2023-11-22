
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
    public TextMeshProUGUI playerPokemonTextName;
    public TextMeshProUGUI enemyPokemonTextName;
    public Slider playerPokemonSlider;
    public Slider enemyPokemonSlider;
    public SpriteRenderer playerPokemonSpriteRenderer;
    public SpriteRenderer enemyPokemonSpriteRenderer;
    [SerializeField] private Camera camera;
    private void Start()
    {
        playerFighterController.fighter.chooseActionEvent = ResolveFight;
        playerFighterController.fighter.endTurnEvent = ChangeTurn;
    }

    public void InitFight(Fighter enemyFighter)
    {
        camera.gameObject.SetActive(true);
        previousSpace = WorldManager.instance.CurrentSpace;
        WorldManager.instance.ChangeSpace(fightSpace);
        enemyFighterController.fighter = enemyFighter;
        enemyFighterController.fighter.chooseActionEvent =ResolveFight;
        enemyFighterController.fighter.endTurnEvent = ChangeTurn;
        playerFighterController.fighter.Init(enemyFighter, playerPokemonSpriteRenderer, enemyPokemonSpriteRenderer, playerPokemonSlider, playerPokemonTextName );
        enemyFighterController.fighter.Init(playerFighterController.fighter, enemyPokemonSpriteRenderer, playerPokemonSpriteRenderer, enemyPokemonSlider, enemyPokemonTextName  );
        isPlayerTurn = false;
        ChangeTurn();
    }
    void ResolveFight()
    {
        playerFighterController.Deactivate();
        enemyFighterController.Deactivate();
    }

    public void ChangeTurn()
    {
     
        enemyFighterController.fighter.RefreshRenderer();
        playerFighterController.fighter.RefreshRenderer();
        
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
        camera.gameObject.SetActive(false);
        enemyFighterController.fighter.Disable();
        playerFighterController.fighter.Disable();
        WorldManager.instance.ChangeSpace(previousSpace);
        
    }
}
