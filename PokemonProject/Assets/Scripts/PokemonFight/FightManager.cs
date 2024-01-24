
using SequencerNS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public PlayerFighterController playerFighterController;
    public EnemyFighterController enemyFighterController;
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
    [SerializeField] private Image playerPokemonStatusBackground;
    [SerializeField] private Image enemyPokemonStatusBackground;
    [SerializeField] private TextMeshProUGUI playerPokemonStatusText;
    [SerializeField] private TextMeshProUGUI enemyPokemonStatusText;
    [SerializeField] private Animator sceneFightAnimator;
    public bool isInFight;
    private void Start()
    {
        playerFighterController.fleeEvent= ExitFight; 
        playerFighterController.fighter.chooseActionEvent = ResolveFight;
        playerFighterController.fighter.endTurnEvent = ChangeTurn;
    }

    public void InitFight(Fighter enemyFighter)
    {
        isInFight = true;
        camera.gameObject.SetActive(true);
        previousSpace = WorldManager.instance.CurrentSpace;
        WorldManager.instance.ChangeSpace(fightSpace);
        enemyFighterController.fighter = enemyFighter;
        enemyFighterController.fighter.chooseActionEvent =ResolveFight;
        enemyFighterController.fighter.endTurnEvent = ChangeTurn;
        Sequencer.Instance.AddCombatInteraction($"{playerFighterController.fighter.GetCurrentPokemonName()} go !" , () =>
        {
            playerFighterController.fighter.Init(enemyFighter, playerPokemonSpriteRenderer, enemyPokemonSpriteRenderer, playerPokemonSlider, playerPokemonTextName, playerPokemonStatusText, playerPokemonStatusBackground, this, sceneFightAnimator );
            playerFighterController.fighter.chooseAnotherPokemonEvent = playerFighterController.ChooseAnotherPokemon;
            ChangeTurn();
        });
        
        enemyFighterController.fighter.Init(playerFighterController.fighter, enemyPokemonSpriteRenderer, playerPokemonSpriteRenderer, enemyPokemonSlider, enemyPokemonTextName, enemyPokemonStatusText, enemyPokemonStatusBackground, this , sceneFightAnimator  );
        isPlayerTurn = false;
    
    }
   public  void ResolveFight()
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

    void ExitFight()
    {
        camera.gameObject.SetActive(false);
        enemyFighterController.fighter.Disable();
        playerFighterController.fighter.Disable();
        WorldManager.instance.ChangeSpace(previousSpace);
        isInFight = false;
    }

 
    
   public void EndFight()
    {
     
        ExitFight();
        
    }
}
