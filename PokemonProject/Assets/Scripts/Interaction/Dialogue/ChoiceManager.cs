using System;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private GameObject choiceHolder;
    public Button[] buttons = new Button[4];

    private void Start()
    {
        choiceHolder.SetActive(false);
    }

   /* public void PopChoiceInteraction(ChoiceSO )
    {
        
    }*/
}
