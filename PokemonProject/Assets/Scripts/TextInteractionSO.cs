using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextInteraction", menuName = "ScriptableObjects/TextInteraction", order = 0)]
public class TextInteractionSO : InteractionSO
{
    [TextArea] public string textToDraw;
}
