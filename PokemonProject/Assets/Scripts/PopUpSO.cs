using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopUp", menuName = "ScriptableObjects/Interaction", order = 0)]
public class PopUpSO : ScriptableObject
{
    private List<InteractionSO> interactions;
    public List<InteractionSO> InteractionsSO => interactions;
}
