using UnityEngine;
 
[CreateAssetMenu(fileName = "Choice", menuName = "ScriptableObjects/ChoiceSO", order = 2)]
public class ChoiceSO : InteractionSO
{
    public ResolutionSO[] choices;
}
