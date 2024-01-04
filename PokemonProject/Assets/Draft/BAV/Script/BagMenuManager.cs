using System.Collections.Generic;
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

    [Header("Items Store")]
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
    }

    
    private void HandleBagSectionChange()
    {
        HandleArrowInput();

        int maxIndex = GetMaxIndex();
        int minIndex = 0;

        HandleUpDownInput(minIndex, maxIndex);

        SetBagSectionText(bagSections[currentBagSectionIndex]);
    }

    private void HandleArrowInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentBagSectionIndex < bagSections.Length - 1)
        {
            currentBagSectionIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentBagSectionIndex > 0)
        {
            currentBagSectionIndex--;
        }
    }

    private void HandleUpDownInput(int minIndex, int maxIndex)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && GetArrowIndex() > minIndex)
        {
            ModifyArrowIndex(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && GetArrowIndex() < maxIndex)
        {
            ModifyArrowIndex(1);
        }
    }

    private int GetMaxIndex()
    {
        switch (currentBagSectionIndex)
        {
            case 0:
                return itemsCommons.Count - 1;
            case 1:
                return keyItems.Count - 1;
            case 2:
                return pokeballItems.Count - 1;
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

        // Set alpha to 1
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
        // Check if ItemType is not KeyItems to display numberOfItems
        if (item.itemType != ItemType.KeyItems)
        {
            // Display "X" and the number of items
            bagItemsCountText[0].text = "X" + " " + GetItemCountText(item);
        }

        bagItemsImage.sprite = item.sprite;
        bagItemsDescriptionText.text = item.description;
    }
    
    public void SetInformationsForItemList(List<ItemSO> itemList, List<TMP_Text> itemTextList, List<TMP_Text> itemCountTextList, Image itemImage, TMP_Text itemDescriptionText)
    {
        EmptyTheBoard();
        int arrowIndex = GetArrowIndex();
        int maxIndex = GetMaxIndex();
        int i = arrowIndex;
        for (int j = 0; j < Mathf.Min(itemList.Count, itemTextList.Count); j++)
        {
            if (arrowIndex > 3)
            {
                i = j + arrowIndex -3;
            }
            else
            {
                i = j;
            }
            // Check if the adjusted index is within the valid range of itemList
            if (i >= 0 && i < itemList.Count)
            {
                itemTextList[j].text = itemList[i].itemName;

                // Check if ItemType is not KeyItems to display numberOfItems
                if (itemList[i].itemType != ItemType.KeyItems)
                {
                    // Display "X" and the number of items
                    itemCountTextList[j].text = "X" + " " + GetItemCountText(itemList[i]);
                }
            }
            else
            {
                // Handle the case where the index is out of range
                itemTextList[j].text = "N/A"; // or any default text
                itemCountTextList[j].text = "X 0"; // or any default text for itemCount
            }
        }

        // Set item-specific image and description for the first item in the list
        if (itemList.Count > 0)
        {
            int firstItemIndex = arrowIndex % itemList.Count;
            itemImage.sprite = itemList[firstItemIndex].sprite;
            itemDescriptionText.text = itemList[firstItemIndex].description;
        }
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
}
