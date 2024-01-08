using System.Collections.Generic;
using SequencerNS;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BagMenuManager : MonoBehaviour
{
    [Header("Bag Section")]
    public TMP_Text bagSectionText;
    public string[] bagSections = { "Items", "Key Items", "Poke Balls"};
    public int currentBagSectionIndex = 0;
    [Header("Bag Items")]
    public List<TMP_Text> bagItemsText;
    public List<TMP_Text> bagItemsCountText;
    [Header("Informations Panels")]
    public Image bagItemsImage;
    public TMP_Text bagItemsDescriptionText;
    
    [Header("Arrow Selector")] 
    public int arrowIndexCommonItem = 0;
    public int arrowIndexKeyItem = 0;
    public int arrowIndexPokeball = 0;
    public Sprite arrowImage;
    public List<Image> arrowsSelectorPosition;

    [Header("Use Item")] 
    public GameObject itemActionMenu;
    public List<Image> arrowImagesItemOptionMenu;
    public GameObject itemSelectedDescription;
    public TMP_Text itemSelectedDescriptionText;
    public int currentArrowPositionItemOptionMenu = 0;
    private bool isItemActionMenuActive = false;

    
    [Header("Items Data Store")]
    public ItemSO itemSODebug;
    public List<ItemSO> itemsCommons = new List<ItemSO>();
    public List<ItemSO> keyItems = new List<ItemSO>();
    public List<ItemSO> pokeballItems = new List<ItemSO>();
    
    [Header("Look base On Character")]
    public Image bagImage;
    public Image backgroundBagMenu;
    public List<Sprite> backgroundSprites = new List<Sprite>();
    public List<Sprite> bagSprites = new List<Sprite>();
    [Range(0,1)]
    [Tooltip("0 is for Man and 1 is for Woman")]
    public int characterIndex = 0;
    
    [SerializeField] private PokemonTeamManager pokemonTeamManager;
    [SerializeField] private FightManager fightManager; 
    
    void Start()
    {
        EmptyTextList(bagItemsText);
        SwitchBackgroundBaseOnCharacter(characterIndex);
    }

    void Update()
    {
        HandleBagSectionChange();

        EnableListOfText(bagItemsCountText);

        switch (currentBagSectionIndex)
        {
            case 0:
                SetInformationsForItemList(itemsCommons, bagItemsText, bagItemsCountText, bagItemsImage, bagItemsDescriptionText);
                break;
            case 1:
                SetInformationsForItemList(keyItems, bagItemsText, bagItemsCountText, bagItemsImage, bagItemsDescriptionText);
                break;
            case 2:
                SetInformationsForItemList(pokeballItems, bagItemsText, bagItemsCountText, bagItemsImage, bagItemsDescriptionText);
                break;
            default:
                // Handle any other cases or log an error
                Debug.LogError($"Unexpected currentBagSectionIndex value: {currentBagSectionIndex}");
                break;
        }


        //Handles Cursor Selector
        DisplayArrow();
        
        //Switch Between Bag Directions
        SwitchBagBaseOnDirection(characterIndex);
        
        //Handles Index Action Menu
    }

    
    private void HandleBagSectionChange()
    {
        HandleArrowInput();

        int maxIndex = GetMaxIndex();
        int minIndex = 0;

        HandleUpDownInput(minIndex, maxIndex);
        HandleButtonAInput(minIndex, maxIndex);
        
        SetBagSectionText(bagSections[currentBagSectionIndex]);
    }

    private void HandleArrowInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isItemActionMenuActive)
        {
            if (currentBagSectionIndex < bagSections.Length - 1)
            {
                currentBagSectionIndex++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isItemActionMenuActive)
        {
            if (currentBagSectionIndex > 0)
            {
                currentBagSectionIndex--;   
            }
        }
    }

    private void HandleUpDownInput(int minIndex, int maxIndex)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isItemActionMenuActive)
            {
                if (GetArrowIndex() > minIndex)
                {
                    ModifyArrowIndex(-1);
                }
            }
            else
            {
                currentArrowPositionItemOptionMenu--;
                if (currentArrowPositionItemOptionMenu < 0)
                {
                    currentArrowPositionItemOptionMenu = arrowImagesItemOptionMenu.Count - 1;
                }
                UpdateArrowPositionPokemonOptionMenu();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!isItemActionMenuActive)
            {
                if (GetArrowIndex() < maxIndex)
                {
                    ModifyArrowIndex(1);
                } 
            }
            else
            {
                currentArrowPositionItemOptionMenu++;
                if (currentArrowPositionItemOptionMenu > arrowImagesItemOptionMenu.Count - 1) //Change this if this is a pokeball or a common Items
                {
                    currentArrowPositionItemOptionMenu = 0;
                }
                UpdateArrowPositionPokemonOptionMenu();
            }
        }
    }

    private void HandleButtonAInput(int minIndex, int maxIndex)
    {
        if (Input.GetKeyDown(KeyCode.A) && Sequencer.Instance.CurrentSequenceType == SequenceType.None)
        {
            if (isItemActionMenuActive)
            {
                HandleArrowIndex();
            }
            else
            {
                if (GetArrowIndex() < maxIndex)
                {
                    DisplayItemOptionMenu();
                }
                else
                {
                    HandleCancelButton();   
                }
            }
        }
    }

    private int GetMaxIndex()
    {
        switch (currentBagSectionIndex)
        {
            case 0:
                return itemsCommons.Count;
            case 1:
                return keyItems.Count;
            case 2:
                return pokeballItems.Count;
            default:
                return 0;
        }
    }



    private int GetArrowIndex()
    {
        switch (currentBagSectionIndex)
        {
            case 0:
                return arrowIndexCommonItem;
            case 1:
                return arrowIndexKeyItem;
            case 2:
                return arrowIndexPokeball;
            default:
                return 0;
        }
    }

    private void ModifyArrowIndex(int amount)
    {
        switch (currentBagSectionIndex)
        {
            case 0:
                ModifyIndex(ref arrowIndexCommonItem, amount);
                break;
            case 1:
                ModifyIndex(ref arrowIndexKeyItem, amount);
                break;
            case 2:
                ModifyIndex(ref arrowIndexPokeball, amount);
                break;
        }
    }

    private void ModifyIndex(ref int index, int amount)
    {
        index += amount;
    }

    private void DisplayArrow()
    {
        EmptyListImage(arrowsSelectorPosition);
        int arrowIndex = GetArrowIndex();
        int maxIndex = GetMaxIndex();
    
        if (arrowIndex is >= 0 and <= 3)
        {
            SetArrowImageAndAlpha(arrowsSelectorPosition[arrowIndex]);
        }
        else if (arrowIndex >= maxIndex - 2)
        {
            int adjustedIndex = arrowIndex >= 0 ? arrowIndex - maxIndex + 5 : 0;
            SetArrowImageAndAlpha(arrowsSelectorPosition[adjustedIndex]);
        }
        else
        {
            SetArrowImageAndAlpha(arrowsSelectorPosition[3]);
        }
    }

    private void SetArrowImageAndAlpha(Image arrowImage)
    {
        arrowImage.sprite = this.arrowImage;

        Color color = arrowImage.color;
        color.a = 1f;
        arrowImage.color = color;
    }


  
    private void EmptyTheBoard()
    {
        EmptyTextList(bagItemsText);
        EmptyTextList(bagItemsCountText);
        EmptyText(bagItemsDescriptionText);
        EmptyImage(bagItemsImage);
    }

    public void SetBagSectionText(string text)
    {
        bagSectionText.text = text;
    }
    
    public void EmptyText(TMP_Text text)
    {
        text.text = "";
    }
    
    public void EmptyImage(Image image)
    {
        image.sprite = null;
    }

    public void EmptyListImage(List<Image> images)
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                Color color = image.color;
                color.a = 0f;
                image.color = color;
                image.sprite = null;
            }
        }
    }

    
    public void EmptyTextList(List<TMP_Text> texts)
    {
        foreach (var item in texts)
        {
            item.text = "";
        }
    }
    
    public void DisableListOfText(List<TMP_Text> texts)
    {
        foreach (var item in texts)
        {
            item.gameObject.SetActive(false);
        }
    }
    
    public void EnableListOfText(List<TMP_Text> texts)
    {
        foreach (var item in texts)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void SetInformationsItemSo(ItemSO item)
    {
        EmptyTextList(bagItemsText);
        bagItemsText[0].text = item.itemName;
        if (item.itemType != ItemType.KeyItems)
        {
            bagItemsCountText[0].text = "X" + " " + GetItemCountText(item);
        }

        bagItemsImage.sprite = item.sprite;
        bagItemsDescriptionText.text = item.description;
    }
    
    public void SetInformationsForItemList(List<ItemSO> itemList, List<TMP_Text> itemTextList, List<TMP_Text> itemCountTextList, Image itemImage, TMP_Text itemDescriptionText)
    {
        EmptyTheBoard();
        int arrowIndex = GetArrowIndex();
        int i = arrowIndex;
    
        for (int j = 0; j < Mathf.Min(itemList.Count +1, itemTextList.Count); j++)
        {
            if (arrowIndex > 3)
            {
                i = j + arrowIndex - 3;
            }
            else
            {
                i = j;
            }
            
            if (i >= 0 && i < itemList.Count)
            {
                itemTextList[j].text = itemList[i].itemName;

                if (itemList[i].itemType != ItemType.KeyItems)
                {
                    itemCountTextList[j].text = "X" + " " + GetItemCountText(itemList[i]);
                }
            }
            else
            {
                itemTextList[j].text = "CANCEL";
                itemCountTextList[j].text = "";
            }
        }

        if (itemList.Count > 0)
        {
            int firstItemIndex = arrowIndex % itemList.Count;
            if (firstItemIndex >= 0 && firstItemIndex < itemList.Count)
            {
                itemImage.sprite = itemList[firstItemIndex].sprite;
                itemDescriptionText.text = itemList[firstItemIndex].description;
            }
        }
    }

    
    public void DisplayItemOptionMenu()
    {
        //Enable Disable some Text
        itemActionMenu.SetActive(true);
        itemSelectedDescription.SetActive(true);
        bagItemsDescriptionText.gameObject.SetActive(false);
        
        //Update the text for the selected Object
        SetItemSelectedDescriptionText();
        
        
        isItemActionMenuActive = true;
        currentArrowPositionItemOptionMenu = 0;
        UpdateArrowPositionPokemonOptionMenu();
    }
    
    private void UpdateArrowPositionPokemonOptionMenu()
    {
        foreach (Image arrowPosition in arrowImagesItemOptionMenu)
        {
            Color color = arrowPosition.color;
            color.a = 0f;
            arrowPosition.color = color;
        }

        Color currentArrowColor = arrowImagesItemOptionMenu[currentArrowPositionItemOptionMenu].color;
        currentArrowColor.a = 1f;
        arrowImagesItemOptionMenu[currentArrowPositionItemOptionMenu].color = currentArrowColor;
    }

    private string GetItemCountText(ItemSO item)
    {
        return item switch
        {
            ItemCommonSO commonItem => commonItem.numberOfItems.ToString(),
            PokeballItemSO pokeballItem => pokeballItem.numberOfItems.ToString(),
            _ => "N/A"
        };
    }
    
    private void SetItemSelectedDescriptionText()
    {
        int selectedIndex = 0;

        switch (currentBagSectionIndex)
        {
            case 0:
                selectedIndex = arrowIndexCommonItem;
                break;
            case 1:
                selectedIndex = arrowIndexKeyItem;
                break;
            case 2:
                selectedIndex = arrowIndexPokeball;
                break;
            default:
                break;
        }

        // Ensure the selected index is within valid bounds
        if (selectedIndex >= 0 && selectedIndex < GetMaxIndex())
        {
            ItemSO selectedItem = GetSelectedItem(selectedIndex);
            if (selectedItem != null)
            {
                itemSelectedDescriptionText.text = selectedItem.itemName + " is " +  "\n" + "selected.";
            }
            else
            {
                itemSelectedDescriptionText.text = "CANCEL";
            }
        }
    }

    private ItemSO GetSelectedItem(int index)
    {
        switch (currentBagSectionIndex)
        {
            case 0:
                return (index >= 0 && index < itemsCommons.Count) ? itemsCommons[index] : null;
            case 1:
                return (index >= 0 && index < keyItems.Count) ? keyItems[index] : null;
            case 2:
                return (index >= 0 && index < pokeballItems.Count) ? pokeballItems[index] : null;
            default:
                return null;
        }
    }

    private void SwitchBackgroundBaseOnCharacter(int index)
    {
        if (index == 0)
        {
            bagImage.sprite = bagSprites[0];
            backgroundBagMenu.sprite = backgroundSprites[0];
        }
        else
        {
            bagImage.sprite = bagSprites[1];
            backgroundBagMenu.sprite = backgroundSprites[1];
        }
    }

    private void SwitchBagBaseOnDirection(int index)
    {
        switch (currentBagSectionIndex)
        {
            case 0:
                bagImage.sprite = bagSprites[1 + index * 3];
                break;
            case 1:
                bagImage.sprite = bagSprites[2 + index * 3];
                break;
            case 2:
                bagImage.sprite = bagSprites[3 + index * 3];
                break;
        }
    }
    
    void HandleArrowIndex()
    {
        switch (currentArrowPositionItemOptionMenu)
        {
            case 0:
                UseItem();
                Debug.Log("Use Item");
                break;
            case 1:
                Debug.Log("Give Item To Pokemon");
                break;
            case 2:
                Debug.Log("Toss the Item");
                break;
            case 3:
                HandleCancelButtonPokemonOptionMenu();
                break;
        }
    }

    void UseItem()
    {
       
        int selectedIndex = 0;
        if (currentBagSectionIndex == 0)
        { 
            selectedIndex = arrowIndexCommonItem;
            if (itemsCommons[selectedIndex] is PotionSO potionSo)
            {
                pokemonTeamManager.OpenPokemonTeamMenuForApplyItem(UsePotion);
                
                void UsePotion(Pokemon pokemon)
                {
                    potionSo.HealPokemon(pokemon);
                    Sequencer.Instance.AddCombatInteraction("Heal Pokemon", () =>
                    {
                        if (fightManager.isInFight)
                        {
                            gameObject.SetActive(false);
                            pokemonTeamManager.gameObject.SetActive(false);
                            fightManager.ChangeTurn();
                        }
                        else
                        {
                            gameObject.SetActive(true);
                            pokemonTeamManager.gameObject.SetActive(false);
                        }
                    });
                }
            }
        }
        else if(currentBagSectionIndex == 2)
        {
            selectedIndex = arrowIndexPokeball;
            if (fightManager.isInFight)
            {
                var pokeball = (PokeballItemSO) pokeballItems[selectedIndex];
                
            }
            void UsePokeball()
            {
                
            }
        }
        gameObject.SetActive(false);
    }


    
    
    
    void HandleCancelButtonPokemonOptionMenu()
    {
        itemActionMenu.SetActive(false);
        itemSelectedDescription.SetActive(false);
        bagItemsDescriptionText.gameObject.SetActive(true);
        isItemActionMenuActive = false;
    }
    
    public void HandleCancelButton()
    {
        ToggleOffTheBagMenu();
    }
    
    public void ToggleOffTheBagMenu()
    {
        gameObject.SetActive(false);
    }
    
}
