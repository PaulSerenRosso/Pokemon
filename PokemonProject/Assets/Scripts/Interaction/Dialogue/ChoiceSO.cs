using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[CreateAssetMenu(fileName = "Choice", menuName = "ScriptableObjects/ChoiceSO", order = 2)]
public class ChoiceSO : InteractionSO
{
    [TextArea] public string textBeforeChoice;
    public InteractionSO[] choices;
}
