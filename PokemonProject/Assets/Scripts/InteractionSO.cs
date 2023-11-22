using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSO : ScriptableObject
{
    public SequenceType interactionType;
    
    [TextArea] public string[] textToDraw;
    public Gender fromGender;
}
