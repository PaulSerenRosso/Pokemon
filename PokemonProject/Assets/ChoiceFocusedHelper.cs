using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceFocusedHelper : MonoBehaviour
{
    public Button startButtonFocused;
    public EventSystem evt;
    
    private void OnEnable()
    {
        evt.SetSelectedGameObject(startButtonFocused.gameObject);
    }
}
