using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PokemonCapacityButtonHover : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private PlayerFighterController playerFighterController;
    [SerializeField] private int index;
    public void OnPointerEnter(PointerEventData eventData)
    {
        playerFighterController.UpdateCapacityInfoPanel(index);
    }
}
