using System.Collections.Generic;
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
    [SerializeField] private TextMeshProUGUI playerPokemonLevelText;
    [SerializeField] private TextMeshProUGUI enemyPokemonLevelText;
    [SerializeField] private GameObject playerUIPanel;
    private List<PokemonWithXpTurn> playerPokemonXpPerTurn = new List<PokemonWithXpTurn>();
    public bool isPlayerWin;
    public bool isInFight;
    public int xpPerTurn = 2;
    public int xpPokemonEntryFight = 3;
    [SerializeField] private Slider sliderXp;
    private void Start()
    {
        playerFighterController.fleeEvent= ExitFight; 
        playerFighterController.fighter.chooseActionEvent = ResolveFight;
        playerFighterController.fighter.endTurnEvent = ChangeTurn;
    }

    public void InitFight(Fighter enemyFighter)
    {
      
        playerFighterController.fighter.initEvent?.Invoke();
        Sequencer.Instance.OnEndSequence = null;
        playerPokemonXpPerTurn.Clear();
        CheckPlayerPokemonXpTurn();
        isInFight = true;
        playerFighterController.fighter.refreshRendererEvent = UpdateXPSlider;
        camera.gameObject.SetActive(true);
        previousSpace = WorldManager.instance.CurrentSpace;
        WorldManager.instance.ChangeSpace(fightSpace);
        enemyFighterController.fighter = enemyFighter;
        enemyFighterController.fighter.chooseActionEvent =ResolveFight;
        enemyFighterController.fighter.endTurnEvent = ChangeTurn;
        playerUIPanel.SetActive(false);
        playerPokemonSpriteRenderer.enabled = false; 
        Sequencer.Instance.AddCombatInteraction($"{playerFighterController.fighter.GetCurrentPokemonName()} go !" , true,() =>
        {
            playerUIPanel.SetActive(true);
            playerPokemonSpriteRenderer.enabled = true; 
            playerFighterController.fighter.Init(enemyFighter, playerPokemonSpriteRenderer, enemyPokemonSpriteRenderer, playerPokemonSlider, playerPokemonTextName, playerPokemonStatusText, playerPokemonStatusBackground, this, sceneFightAnimator, playerPokemonLevelText, playerPokemonSpriteRenderer );
            playerFighterController.fighter.chooseAnotherPokemonEvent = playerFighterController.ChooseAnotherPokemon;
            ChangeTurn();
        });
        
        enemyFighterController.fighter.Init(playerFighterController.fighter, enemyPokemonSpriteRenderer, playerPokemonSpriteRenderer, enemyPokemonSlider, enemyPokemonTextName, enemyPokemonStatusText, enemyPokemonStatusBackground, this , sceneFightAnimator , enemyPokemonLevelText );
        isPlayerTurn = false;
    
    }
   public  void ResolveFight()
    {
        playerFighterController.Deactivate();
        enemyFighterController.Deactivate();
    }

    public void ChangeTurn()
    {

        if (isPlayerTurn)
        {
            CheckPlayerPokemonXpTurn();
        }
        if (enemyFighterController.fighter.CheckLose())
        {
            isPlayerWin = true;
            EndFight();
            return; 
        }
        if(playerFighterController.fighter.CheckLose())
        {
            isPlayerWin = false;
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
        enemyFighterController.fighter.pokemons.Clear();
        camera.gameObject.SetActive(false);
        enemyFighterController.fighter.Disable();
        playerFighterController.fighter.Disable();
        WorldManager.instance.ChangeSpace(previousSpace);
        isInFight = false;
        Sequencer.Instance.OnEndSequence = null;
    }

    private void CheckPlayerPokemonXpTurn()
    {
        Fighter fighter = playerFighterController.fighter;
        bool isContained = false;
        foreach (var pokemon in playerPokemonXpPerTurn )
        {
            if (pokemon.pokemon == fighter.pokemons[fighter.currentPokemonIndex])
            {
                isContained = true;
                pokemon.xpTurn++;
                break;
            }
        }

        if (!isContained)
        {
            playerPokemonXpPerTurn.Add(new PokemonWithXpTurn(fighter.pokemons[fighter.currentPokemonIndex]));
        }
    }
    
   public void EndFight()
    {

        if (isPlayerWin)
        {
            TryAddPlayerPokemonXP(0);
        }
        else
        {
            ExitFight();
            
        }
    }

   private void TryAddPlayerPokemonXP(int index)
   {
       var fighter = playerFighterController.fighter;
       Debug.Log(index);
       if (playerPokemonXpPerTurn.Count == index)
       {
           ExitFight();
           return;
       }
       var currentPokemon = playerPokemonXpPerTurn[index].pokemon;
       if (!currentPokemon.IsDied)
       {
           var xpWon = xpPokemonEntryFight + xpPerTurn * playerPokemonXpPerTurn[index].xpTurn;
           var isLevelUp = currentPokemon.IncreaseXP(xpWon);
           if (playerPokemonXpPerTurn[index].pokemon == fighter.pokemons[fighter.currentPokemonIndex])
           {
               UpdateXPSlider();
           }
           Sequencer.Instance.AddCombatInteraction($"{currentPokemon.so.name} gains {xpWon} XP ! ",true,()=>
           {
               if (isLevelUp)
               {
                   AddPlayerPokemonLevelUp(index);
               }
               else
               {
                   index += 1;
                   TryAddPlayerPokemonXP(index);
               }
           });
       }
       else
       {
           index += 1;
           TryAddPlayerPokemonXP(index);
       }
   }

   private void UpdateXPSlider()
   {
       var fighter = playerFighterController.fighter;  
       sliderXp.value = ((float)fighter.pokemons[fighter.currentPokemonIndex].Xp)/fighter.pokemons[fighter.currentPokemonIndex].MaxXP;
   }

   private void AddPlayerPokemonLevelUp(int index)
   {
       Debug.Log("AddPlayerPokemonLevelUp Trigger");
       var fighter = playerFighterController.fighter;
       var currentPokemon = playerPokemonXpPerTurn[index].pokemon;
       if (playerPokemonXpPerTurn[index].pokemon == fighter.pokemons[fighter.currentPokemonIndex])
       {
           fighter.RefreshRenderer();
       }
       Sequencer.Instance.AddCombatInteraction($"{currentPokemon.so.name} level to {currentPokemon.Level} ",true, () =>
       {
           index += 1;
           TryAddPlayerPokemonXP((index));
       });
   }
}
