using System.Collections;
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

    [Header("Items Store")]
    public ItemSO itemSODebug;
    public List<ItemSO> itemsCommons = new List<ItemSO>();
    public List<ItemSO> keyItems = new List<ItemSO>();
    public List<ItemSO>  pokeballItems = new List<ItemSO>();
    
    void Start()
    {
        EmptyTextList(bagItemsText);
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

    }

    
    private void HandleBagSectionChange()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentBagSectionIndex < bagSections.Length - 1)
        {
            currentBagSectionIndex++;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentBagSectionIndex > 0)
        {
            currentBagSectionIndex--;
        }

        SetBagSectionText(bagSections[currentBagSectionIndex]);
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

        for (int i = 0; i < Mathf.Min(itemList.Count, itemTextList.Count); i++)
        {
            itemTextList[i].text = itemList[i].itemName;

            // Check if ItemType is not KeyItems to display numberOfItems
            if (itemList[i].itemType != ItemType.KeyItems)
            {
                // Display "X" and the number of items
                itemCountTextList[i].text = "X" + " " + GetItemCountText(itemList[i]);
            }
        }
        
        // Set item-specific image and description for the first item in the list
        if (itemList.Count > 0)
        {
            itemImage.sprite = itemList[0].sprite;
            itemDescriptionText.text = itemList[0].description;
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
}
