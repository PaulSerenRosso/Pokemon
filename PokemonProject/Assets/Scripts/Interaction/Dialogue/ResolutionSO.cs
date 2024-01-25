using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Resolution", menuName = "ScriptableObjects/ResolutionSO", order = 3)]
public class ResolutionSO : InteractionSO
{
    [Space]
    [Header("Resolution Section")]
    public string title;

    public ResolutionsInteractions resolutionType = ResolutionsInteractions.CloseBox;
    
    public PokemonSO pokemonToGive;
    
    public enum ResolutionsInteractions
    {
        CloseBox,
        PokemonGiver,
        Text,
        SetName,
        SetGender
    }
}
