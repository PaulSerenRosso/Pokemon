
using System;
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
    private List<PokemonWithXpTurn> playerPokemonXpPerTurn = new List<PokemonWithXpTurn>();
    private bool isPlayerWin;
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
        Sequencer.Instance.AddCombatInteraction($"{playerFighterController.fighter.GetCurrentPokemonName()} go !" , () =>
        {
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
        camera.gameObject.SetActive(false);
        enemyFighterController.fighter.Disable();
        playerFighterController.fighter.Disable();
        WorldManager.instance.ChangeSpace(previousSpace);
        isInFight = false;
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
            EndFight();
        }
      
        
    }

   private void TryAddPlayerPokemonXP(int index)
   {
       var fighter = playerFighterController.fighter;
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
          
           Sequencer.Instance.AddCombatInteraction($"{currentPokemon.so.name} gains {xpWon} ",()=>
           {
               if (playerPokemonXpPerTurn[index].pokemon == fighter.pokemons[fighter.currentPokemonIndex])
               {
                   UpdateXPSlider();
               }

               if (isLevelUp)
               {
                   AddPlayerPokemonLevelUp(index);
               }
               else
               {
                   TryAddPlayerPokemonXP((index++));
               }
             
               Debug.Log("No issues");
           });
           
       }
   }

   private void UpdateXPSlider()
   {
       var fighter = playerFighterController.fighter;  
       sliderXp.value = (float)fighter.pokemons[fighter.currentPokemonIndex].Xp/fighter.pokemons[fighter.currentPokemonIndex].MaxHp;
   }

   private void AddPlayerPokemonLevelUp(int index)
   {
       Debug.Log("AddPlayerPokemonLevelUp Trigger");
       var fighter = playerFighterController.fighter;
       var currentPokemon = playerPokemonXpPerTurn[index].pokemon;
       Sequencer.Instance.AddCombatInteraction($"{currentPokemon.so.name} level to {currentPokemon.Level} ", () =>
       {
           if (playerPokemonXpPerTurn[index].pokemon == fighter.pokemons[fighter.currentPokemonIndex])
           {
               fighter.RefreshRenderer();
           }
           TryAddPlayerPokemonXP((index++)); 
           
       });
   }
}
